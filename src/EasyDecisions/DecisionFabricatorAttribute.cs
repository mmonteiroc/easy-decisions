using System;

namespace EasyDecisions;

/// <summary>
/// Specifies the name of the decision that this class fabricates.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class DecisionFabricatorAttribute : Attribute
{
    public string Name { get; }

    public DecisionFabricatorAttribute(string name)
    {
        Name = name;
    }
}
