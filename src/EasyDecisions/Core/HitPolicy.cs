namespace EasyDecisions;

/// <summary>
/// Defines how multiple matching rules in a decision table are handled.
/// </summary>
public enum HitPolicy
{
    /// <summary>
    /// Every matching rule is applied in the order they were defined.
    /// This is the default policy.
    /// </summary>
    Collect,

    /// <summary>
    /// Only the first matching rule is applied. Rule evaluation stops after the first match.
    /// </summary>
    First,

    /// <summary>
    /// Only one rule is allowed to match. If more than one rule matches, an exception is thrown.
    /// </summary>
    Unique
}
