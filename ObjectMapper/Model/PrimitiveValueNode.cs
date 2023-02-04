using ReflectionsTest.ObjectMapper.Visitors;

namespace ReflectionsTest.ObjectMapper.Model;

internal sealed class PrimitiveValueNode : ReflectionNodeBase
{
    public PrimitiveValueNode(object? value)
        : base() => Value = value;

    public object? Value { get; }

    public override TypeClassification TypeClassification => TypeClassification.Primitive;

    public override void Accept(IVisitor<ReflectionNodeBase> visitor) => visitor.VisitPrimitiveValueNode(this);
}