using System;

namespace EasyDecisions;

/// <summary>
/// Specifies the name of the decision that this class provides.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class DecisionAttribute : Attribute
{
    public string Name { get; }

    public DecisionAttribute(string name)
    {
        Name = name;
    }
}

/// <summary>
/// Obsolete. Use <see cref="DecisionAttribute"/> instead.
/// </summary>
[Obsolete("Use DecisionAttribute instead.")]
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class DecisionFabricatorAttribute : DecisionAttribute
{
    public DecisionFabricatorAttribute(string name) : base(name)
    {
    }
}
