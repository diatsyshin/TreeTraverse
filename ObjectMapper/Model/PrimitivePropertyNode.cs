using ReflectionsTest.ObjectMapper.Visitors;

namespace ReflectionsTest.ObjectMapper.Model;

internal sealed class PrimitivePropertyNode : ReflectionNodeBase
{
    public PrimitivePropertyNode(ReflectionNodeBase? parent, object valueOwner, IProperty property)
        : base(parent) 
    {
        ValueOwnerReference = valueOwner;
        Property = property;
    }

    public object ValueOwnerReference { get; }

    public IProperty Property { get; }

    public override TypeClassification TypeClassification => TypeClassification.Primitive;

    public override void Accept(IVisitor<ReflectionNodeBase> visitor) => visitor.VisitPrimitivePropertyNode(this, ValueOwnerReference);
}