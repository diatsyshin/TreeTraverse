#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ReflectionsTest.ObjectMapper.Visitors;

namespace ReflectionsTest.ObjectMapper.Model;

internal sealed class ObjectPropertyNode : ReflectionNodeBase
{
    public ObjectPropertyNode(object propertyOwnerReference, IProperty property, List<ReflectionNodeBase> properties)
    {
        PropertyOwnerReference = propertyOwnerReference;
        Property = property;
        Properties = properties.AsReadOnly();
    }

    public override TypeClassification TypeClassification => TypeClassification.RefType;

    public object PropertyOwnerReference { get; }
    
    public IProperty Property { get; }
    
    public IReadOnlyCollection<ReflectionNodeBase> Properties { get; }

    public override void Accept(IVisitor<ReflectionNodeBase> visitor)
    {
        throw new NotImplementedException();
    }
}
