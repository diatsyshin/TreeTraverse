#nullable enable

using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

internal interface IProperty {
    Type InstanceType { get; }
    Type PropertyType { get; }

    public IPropertySetter? Setter { get; }
    public IPropertyGetter? Getter { get; }
}

internal sealed class Property : IProperty
{
    public Property(Type instanceType, Type propertyType, IPropertySetter? setter, IPropertyGetter? getter)
    {
        InstanceType = instanceType;
        PropertyType = propertyType;

        Setter = setter;
        Getter = getter;
    }

    public Type InstanceType { get; }

    public Type PropertyType { get; }

    public IPropertySetter? Setter { get; }

    public IPropertyGetter? Getter { get; }
}

internal interface IPropertyGetter {
    object? GetValue(object instance);
}

internal interface IPropertyGetter<T, TValue> : IPropertyGetter {
    TValue? GetValue(T instance);
}

internal sealed class PropertyGetter<T, TValue> : IPropertyGetter<T, TValue>, IPropertyGetter {
    private readonly Func<T, TValue> _getter;

    public PropertyGetter(Func<T, TValue> getter) => _getter = getter;

    public TValue GetValue(T instance) => _getter.Invoke(instance);

    object? IPropertyGetter.GetValue(object instance) => this.GetValue((T)instance);
}

internal interface IPropertySetter {
    void SetValue(object instance, object? value);
}

internal interface IPropertySetter<T, TValue>: IPropertySetter {
    void SetValue(T instance, TValue? value);
}

internal sealed class PropertySetter<T, TValue> : IPropertySetter<T, TValue>, IPropertySetter {
    private readonly Action<T, TValue> _setterAction;

    public PropertySetter(Action<T, TValue> setterAction) => _setterAction = setterAction;

    public void SetValue(T instance, TValue? value) => _setterAction.Invoke(instance, value);

    void IPropertySetter.SetValue(object instance, object? value) => this.SetValue((T)instance, (TValue?)value);
}


internal static class ExpressionExt {
    public static IPropertySetter<T, TValue> CreateSetter<T, TValue>(this T instance, string propName) 
        where T: class
    {
        var type = typeof(T);
        var targetProp = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.Name.Equals(propName)).SingleOrDefault() ?? throw new ArgumentException("Target property not found");

        return InternalCreateSetter<T, TValue>(targetProp);
    }

    public static IPropertySetter CreateSetter(this PropertyInfo targetProp) 
    {
        var instanceType = targetProp.DeclaringType;
        var propertyType = targetProp.PropertyType;

        Debug.Assert(instanceType is not null && propertyType is not null);

        var createMethod = typeof(ExpressionExt).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
            .Where(m => m.Name.Equals(nameof(InternalCreateSetter)))
            .SingleOrDefault() ?? throw new ArgumentException("Create setter method not found");

        var genericCreateMethiod = createMethod.MakeGenericMethod(instanceType, propertyType);

        var instance = (IPropertySetter?)genericCreateMethiod.Invoke(null, new object?[] { targetProp });

        Debug.Assert(instance is not null);

        return instance;
    }

    public static IPropertyGetter CreateGetter(this PropertyInfo targetProperty) {
        var instanceType = targetProperty.DeclaringType;
        var propertyType = targetProperty.PropertyType;

        Debug.Assert(instanceType is not null && propertyType is not null);

        var createMethod = typeof(ExpressionExt).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
            .Where(m => m.Name.Equals(nameof(InternalCreateGetter)))
            .SingleOrDefault() ?? throw new ArgumentException("Create getter method not found");

        var genericCreateMethiod = createMethod.MakeGenericMethod(instanceType, propertyType);

        var instance = (IPropertyGetter?)genericCreateMethiod.Invoke(null, new object?[] { targetProperty });

        Debug.Assert(instance is not null);

        return instance;
    }

    public static IPropertySetter<T, TValue> CreateSetter<T, TValue>(this T target, PropertyInfo targetProp)
        where T : class => InternalCreateSetter<T, TValue>(targetProp);

    public static IPropertyGetter<T, TValue> CreateGetter<T, TValue>(this T target, PropertyInfo targetProp) 
        where T : class => InternalCreateGetter<T, TValue>(targetProp);

    private static IPropertySetter<T, TValue> InternalCreateSetter<T, TValue>(PropertyInfo targetProperty) {
        var instanceTypeParam = Expression.Parameter(typeof(T), "inst");
        var propTypeParam = Expression.Parameter(typeof(TValue), "newvalue");

        var setterMethodInfo = targetProperty.GetSetMethod();
        
        Debug.Assert(setterMethodInfo is not null);

        var callSetterExpr = Expression.Call(instanceTypeParam, setterMethodInfo, propTypeParam);
        var setPropValueExpr =  Expression.Lambda(callSetterExpr, instanceTypeParam, propTypeParam);
        var setterLambda = (Expression<Action<T, TValue>>)setPropValueExpr;

        var setterType = typeof(PropertySetter<,>);
        var genericType = setterType.MakeGenericType(typeof(T), typeof(TValue));

        var setterInstance = (IPropertySetter<T, TValue>?)Activator.CreateInstance(genericType, setterLambda.Compile());

        Debug.Assert(setterInstance is not null);

        return setterInstance;
    }

    private static IPropertyGetter<T, TValue> InternalCreateGetter<T, TValue>(PropertyInfo targetProperty) {
        var instanceTypeParam = Expression.Parameter(typeof(T), "inst");

        var getterMethodInfo = targetProperty.GetGetMethod();
        
        Debug.Assert(getterMethodInfo is not null);

        var callGetterExpr = Expression.Call(instanceTypeParam, getterMethodInfo);
        var getPropValueExpr =  Expression.Lambda(callGetterExpr, instanceTypeParam);
        var getterLambda = (Expression<Func<T, TValue>>)getPropValueExpr;

        var setterType = typeof(PropertyGetter<,>);
        var genericType = setterType.MakeGenericType(typeof(T), typeof(TValue));

        var instance = (IPropertyGetter<T, TValue>?)Activator.CreateInstance(genericType, getterLambda.Compile());

        Debug.Assert(instance is not null);

        return instance;
    }
}