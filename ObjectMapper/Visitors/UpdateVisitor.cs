using ReflectionsTest.ObjectMapper.Model;

namespace ReflectionsTest.ObjectMapper.Visitors;

internal sealed class UpdaterVisitor : IVisitor<ReflectionNodeBase>
{
    public void Visit(ReflectionNodeBase instance) => instance.Accept(this);

    public void VisitCollectionNode(CollectionNode node){
        foreach(var item in node.Items) {
            item.Accept(this);
        }
    }

    public void VisitObjectNode(ObjectNode node) {
        foreach(var prop in node.Properties) {
            prop.Accept(this);
        }
    }

    public void VisitPrimitivePropertyNode(PrimitivePropertyNode node, object valueOwner) {
        var value = node.Property.Getter.GetValue(valueOwner);
        if(node.Property.PropertyType == typeof(double?)) {
            node.Property.Setter.SetValue(valueOwner, ((double?)value ?? 0.0) + 1.0);
        }

        if(node.Property.PropertyType == typeof(int)) {
            node.Property.Setter.SetValue(valueOwner, (int)value + 1);
        }

        if(node.Property.PropertyType == typeof(string)) {
            node.Property.Setter.SetValue(valueOwner, "Updated");
        }
    }

    public void VisitPrimitiveValueNode(PrimitiveValueNode node) {
    }

    public void VisitNullNode(NullNode node)
    {
    }
}
