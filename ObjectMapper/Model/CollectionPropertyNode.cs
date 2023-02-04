#nullable enable

using ReflectionsTest.ObjectMapper.Visitors;
using System;
using System.Collections.Generic;

namespace ReflectionsTest.ObjectMapper.Model;

internal sealed class CollectionPropertyNode : ReflectionNodeBase
{
    public CollectionPropertyNode(object propertyOwnerReference, IProperty property, List<ReflectionNodeBase> items)
    {
        PropertyOwnerReference = propertyOwnerReference;
        Property = property;
        Items = items.AsReadOnly();
    }

    public override TypeClassification TypeClassification => TypeClassification.Collection;

    public object PropertyOwnerReference { get; }
    
    public IProperty Property { get; }
    
    public IReadOnlyCollection<ReflectionNodeBase> Items { get; }

    public override void Accept(IVisitor<ReflectionNodeBase> visitor)
    {
        throw new NotImplementedException();
    }
}
