
using ReflectionsTest.ObjectMapper.Visitors;

namespace ReflectionsTest.ObjectMapper.Model;

internal sealed class NullNode : ReflectionNodeBase
{
    public NullNode()
    { }

    public override TypeClassification TypeClassification => TypeClassification.RefType;

    public override void Accept(IVisitor<ReflectionNodeBase> visitor) => visitor.VisitNullNode(this);
}