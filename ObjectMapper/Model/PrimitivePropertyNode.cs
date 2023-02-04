using ReflectionsTest.ObjectMapper.Visitors;

namespace ReflectionsTest.ObjectMapper.Model;

internal sealed class PrimitivePropertyNode : ReflectionNodeBase
{
    public PrimitivePropertyNode(object valueOwner, IProperty property)
        : base()
    {
        PropertyOwnerReference = valueOwner;
        Property = property;
    }

    public object PropertyOwnerReference { get; }

    public IProperty Property { get; }

    public override TypeClassification TypeClassification => TypeClassification.Primitive;

    public override void Accept(IVisitor<ReflectionNodeBase> visitor) => visitor.VisitPrimitivePropertyNode(this, PropertyOwnerReference);
}