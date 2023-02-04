using ReflectionsTest.ObjectMapper.Visitors;

namespace ReflectionsTest.ObjectMapper.Model;

internal abstract class ReflectionNodeBase {
    public abstract TypeClassification TypeClassification { get; }

    public abstract void Accept(IVisitor<ReflectionNodeBase> visitor);
}