using System;

namespace EasyDecisions.Exceptions;

/// <summary>
/// Exception thrown when multiple decision fabricators are found for the given input/output types.
/// </summary>
public class AmbiguousDecisionException : Exception
{
    public AmbiguousDecisionException(string message) : base(message)
    {
    }
}
