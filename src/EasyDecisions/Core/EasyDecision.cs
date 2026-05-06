using System;
using System.Reflection;

namespace EasyDecisions;



/// <summary>
/// A helper class providing fully type-safe alternatives for evaluating decisions.
/// </summary>
public static class EasyDecision
{
    /// <summary>
    /// Evaluates the decision in a fully type-safe manner.
    /// </summary>
    public static TOutput Evaluate<TDecision, TInput, TOutput>(TInput input) 
        where TDecision : Decision<TInput, TOutput>, new()
        where TOutput : new()
    {
        return new TDecision().Evaluate(input);
    }

    /// <summary>
    /// Instantiates a decision so that it can be evaluated directly.
    /// </summary>
    public static TDecision Create<TDecision>() where TDecision : class, new()
    {
        return new TDecision();
    }
}
