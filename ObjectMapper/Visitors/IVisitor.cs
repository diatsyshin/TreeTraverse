using ReflectionsTest.ObjectMapper.Model;

namespace ReflectionsTest.ObjectMapper.Visitors;

internal interface IVisitor<TNode>
    where TNode: ReflectionNodeBase
{
    void Visit(TNode instance);
    
    void VisitCollectionNode(CollectionNode node);

    void VisitObjectNode(ObjectNode node);

    void VisitPrimitivePropertyNode(PrimitivePropertyNode node, object valueOwner);

    void VisitPrimitiveValueNode(PrimitiveValueNode node);
}