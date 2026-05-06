using System;

namespace EasyDecisions.Exceptions;

/// <summary>
/// Exception thrown when a decision fabricator is not registered or found for the given input/output types.
/// </summary>
public class DecisionNotRegisteredException : Exception
{
    public DecisionNotRegisteredException(string message) : base(message)
    {
    }
}
