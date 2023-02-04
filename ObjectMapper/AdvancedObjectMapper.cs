#nullable enable

using System.Collections;
using System.Diagnostics;
using System.Reflection;
using ReflectionsTest.ObjectMapper.Model;

internal sealed class AdvancedObjectMapper<T>
    where T: class
{
    private Dictionary<Type, Dictionary<string, IProperty>> _propertiesCache = new ();

    public ReflectionNodeBase BuildMap(T? instance)
    {
        if(instance is null) return new NullNode();

        return HandleObject(instance);
    }

    private ReflectionNodeBase HandleObject(object instance) {
        var type = instance.GetType();
        var classification = GetTypeClassification(type);
        
        ReflectionNodeBase tree = classification switch 
        {
            TypeClassification.Collection   => HandleCollection((IEnumerable)instance),
            TypeClassification.RefType      => HandleRefType(instance),
            TypeClassification.Primitive    => HandlePrimitiveValue(instance),
            
            _ => throw new ArgumentException("Unknown type classification")
        };
        return tree;
    }

    private ReflectionNodeBase HandleProperty(object target, IProperty property) {
        var value = property.Getter.GetValue(target);
        var classification = GetTypeClassification(property.PropertyType);
        ReflectionNodeBase node = classification switch 
        {
            TypeClassification.Collection   => HandleCollection((IEnumerable)value),
            TypeClassification.RefType      => HandleRefType(value),
            TypeClassification.Primitive    => HandlePrimitiveProperty(target, property),
            
            _ => throw new ArgumentException("Unknown type classification")
        };
        return node;
    }

    private CollectionNode HandleCollection(IEnumerable collection) {
        var items = new List<ReflectionNodeBase>();
        foreach(var item in collection) {
            if (item is null) continue;
            
            var node = HandleObject(item);

            items.Add(node);
        }
        return new CollectionNode(items);
    }

    private ReflectionNodeBase HandleRefType(object? target)
    {
        if(target is null) return new NullNode();
        
        var props = new List<ReflectionNodeBase>();
        var properties = target.GetType().GetProperties();
        foreach(var prop in properties) {
            var property    = GetOrCreateProperty(prop);
            var node        = HandleProperty(target, property);

            props.Add(node);
        }
        return new ObjectNode(target, props);
    }

    private PrimitiveValueNode HandlePrimitiveValue(object? value) => new PrimitiveValueNode(value);

    private PrimitivePropertyNode HandlePrimitiveProperty(object propertyOwner, IProperty property) => new PrimitivePropertyNode(propertyOwner, property);
    

    private static TypeClassification GetTypeClassification(Type type) {
        if(type.IsCollection()) return TypeClassification.Collection;
        if(!type.IsValueType && (type != typeof(String))) return TypeClassification.RefType;
        if(type.IsPrimitive()) return TypeClassification.Primitive;
        
        return TypeClassification.Unknown;
    }

    private IProperty GetOrCreateProperty(PropertyInfo prop)
    {
        IProperty? property = null;
        if (!TryGetPropertyFromCache(prop, out property))
        {
            var setter = prop.CreateSetter();
            var getter = prop.CreateGetter();

            property = new Property(prop.DeclaringType, prop.PropertyType, setter, getter);

            AddToCache(prop, property);
        }

        Debug.Assert(property is not null);

        return property;
    }

    private bool TryGetPropertyFromCache(PropertyInfo prop, out IProperty? property)
    {
        //Init cache
        if (!_propertiesCache.ContainsKey(prop.DeclaringType))
        {
            _propertiesCache[prop.DeclaringType] = new();
        }
        property = IsPropertyInCache(prop)
            ? _propertiesCache[prop.DeclaringType][prop.Name]
            : null;

        return property is not null;
    }

    private bool IsPropertyInCache(PropertyInfo prop) =>
        _propertiesCache.ContainsKey(prop.DeclaringType)
                    && _propertiesCache[prop.DeclaringType].ContainsKey(prop.Name);

    private void AddToCache(PropertyInfo prop, IProperty property) => _propertiesCache[prop.DeclaringType][prop.Name] = property;
}