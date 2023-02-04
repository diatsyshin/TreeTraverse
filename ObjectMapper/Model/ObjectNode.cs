
using ReflectionsTest.ObjectMapper.Visitors;

namespace ReflectionsTest.ObjectMapper.Model;

internal sealed class ObjectNode : ReflectionNodeBase
{
    public ObjectNode(object objectReference, List<ReflectionNodeBase> properties)
    {
        Properties = properties;
        ObjectReference = objectReference;
    }

    public IReadOnlyCollection<ReflectionNodeBase> Properties { get; }
    
    public object ObjectReference { get; }

    public override TypeClassification TypeClassification => TypeClassification.RefType;

    public override void Accept(IVisitor<ReflectionNodeBase> visitor) => visitor.VisitObjectNode(this);
}