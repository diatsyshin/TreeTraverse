using ReflectionsTest.ObjectMapper.Model;

namespace ReflectionsTest.ObjectMapper.Visitors;

internal sealed class SimpleVisitor : IVisitor<ReflectionNodeBase>
{
    public void Visit(ReflectionNodeBase instance) => instance.Accept(this);

    public void VisitCollectionNode(CollectionNode node) {
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
        //Console.WriteLine(node?.Property?.Getter?.GetValue(valueOwner));
    }

    public void VisitPrimitiveValueNode(PrimitiveValueNode node) {
        //Console.WriteLine("Primitive node value: {0}", node.Value);
    }
}