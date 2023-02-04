using ReflectionsTest.ObjectMapper.Visitors;

namespace ReflectionsTest.ObjectMapper.Model;

internal sealed class CollectionNode : ReflectionNodeBase
{
    public IReadOnlyCollection<ReflectionNodeBase> Items { get; }

    public override TypeClassification TypeClassification => TypeClassification.Collection;

    public CollectionNode(List<ReflectionNodeBase> items)
        :base() => Items = items;

    public override void Accept(IVisitor<ReflectionNodeBase> visitor) => visitor.VisitCollectionNode(this);
}
