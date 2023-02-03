using ReflectionsTest.ObjectMapper.Visitors;

namespace ReflectionsTest.ObjectMapper.Model;

internal abstract class ReflectionNodeBase {
    public ReflectionNodeBase? Parent { get; set; }

    public abstract TypeClassification TypeClassification { get; }

    protected ReflectionNodeBase() :this(null) {}

    protected ReflectionNodeBase(ReflectionNodeBase? parent) => Parent = parent;

    public abstract void Accept(IVisitor<ReflectionNodeBase> visitor);
}