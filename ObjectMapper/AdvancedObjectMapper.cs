#nullable enable

using System.Collections;
using ReflectionsTest.ObjectMapper.Model;

internal sealed class AdvancedObjectMapper<T>
    where T: class
{
    private Dictionary<Type, Dictionary<string, IProperty>> _propertiesCache = new ();

    public ReflectionNodeBase? BuildMap(T? instance)
    {
        if(instance is null) return null;

        return HandleObject(instance);
    }

    private ReflectionNodeBase HandleObject(object instance) {
        var type = instance.GetType();
        var classification = GetTypeClassification(type);
        
        ReflectionNodeBase tree = classification switch 
        {
            TypeClassification.Collection   => HandleCollection(null, (IEnumerable)instance),
            TypeClassification.RefType      => HandleRefType(null, instance),
            TypeClassification.Primitive    => HandlePrimitiveValue(null, instance),
            
            _ => throw new ArgumentException("Unknown type classification")
        };
        return tree;
    }

    private ReflectionNodeBase HanleProperty(object target, IProperty property) {
        var value = property.Getter.GetValue(target);
        var classification = GetTypeClassification(value.GetType());
        ReflectionNodeBase node = classification switch 
        {
            TypeClassification.Collection   => HandleCollection(null, (IEnumerable)value),
            TypeClassification.RefType      => HandleRefType(null, value),
            TypeClassification.Primitive    => HandlePrimitiveProperty(null, target, property),
            
            _ => throw new ArgumentException("Unknown type classification")
        };
        return node;
    }

    private CollectionNode HandleCollection(ReflectionNodeBase? parent, IEnumerable collection) {
        var items = new List<ReflectionNodeBase>();
        foreach(var item in collection) {
            if (item is null) continue;
            
            var node = HandleObject(item);

            items.Add(node);
        }
        return new CollectionNode(parent, items);
    }

    private ObjectNode HandleRefType(ReflectionNodeBase? parent, object target)
    {
        var props = new List<ReflectionNodeBase>();
        var properties = target.GetType().GetProperties();
        foreach(var prop in properties) {
            IProperty? property = null;
            if(_propertiesCache.ContainsKey(prop.DeclaringType) 
                && _propertiesCache[prop.DeclaringType].ContainsKey(prop.Name)) 
            {
                property = _propertiesCache[prop.DeclaringType][prop.Name];
            } else {
                var setter = prop.CreateSetter();
                var getter = prop.CreateGetter();
                
                property = new Property(prop.DeclaringType, prop.PropertyType, setter, getter);

                if(!_propertiesCache.ContainsKey(prop.DeclaringType)){
                    _propertiesCache[prop.DeclaringType] = new ();
                }
                
                _propertiesCache[prop.DeclaringType][prop.Name] = property;
            }

            var node = HanleProperty(target, property);

            props.Add(node);
        }
        return new ObjectNode(parent, target, props);
    }

    private PrimitiveValueNode HandlePrimitiveValue(ReflectionNodeBase? parent, object? value) => new PrimitiveValueNode(parent, value);

    private PrimitivePropertyNode HandlePrimitiveProperty(ReflectionNodeBase? parent, object valueOwner, IProperty property) => new PrimitivePropertyNode(parent, valueOwner, property);
    

    private static TypeClassification GetTypeClassification(Type type) {
        if(type.IsCollection()) return TypeClassification.Collection;
        if(!type.IsValueType && (type != typeof(String))) return TypeClassification.RefType;
        if(type.IsPrimitive()) return TypeClassification.Primitive;
        
        return TypeClassification.Unknown;
    }
}