
using System.Collections;
using System.Reflection;

public static class ReflectionHelpersExtensions
{
    public static bool IsPrimitive(this PropertyInfo propertyInfo) => propertyInfo.PropertyType.IsPrimitive();
    
    public static bool IsNullable(this PropertyInfo propertyInfo) => propertyInfo.PropertyType.IsNullable();
    
    public static bool IsCollection(this PropertyInfo propertyInfo) => propertyInfo.PropertyType.IsCollection();

    public static bool IsNullable(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

    public static bool IsPrimitive(this Type type) => type.IsPrimitive || (type.Namespace != null && type.Namespace.StartsWith("System"));

    public static bool IsCollection(this Type type)
    {
        if(type == typeof(String)) return false;

        if (typeof(IEnumerable).IsAssignableFrom(type) ||
            typeof(IDictionary).IsAssignableFrom(type))
        {
            return true;
        }

        if (type.IsGenericType)
        {
            Type[] genericInterfaces = type.GetInterfaces();
            foreach (Type interfaceType in genericInterfaces)
            {
                if (interfaceType.IsGenericType &&
                    (interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
                    interfaceType.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
                {
                    return true;
                }
            }
        }

        return false;
    }
}