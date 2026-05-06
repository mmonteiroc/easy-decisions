using System;
using System.Reflection;

namespace EasyDecisions;



/// <summary>
/// A helper class providing fully type-safe alternatives for evaluating decisions.
/// </summary>
public static class EasyDecision
{
    /// <summary>
    /// Evaluates the decision by only specifying the decision class.
    /// The input type is inferred, and the output type is determined at runtime (dynamic).
    /// </summary>
    /// <typeparam name="TDecision">The decision class to evaluate.</typeparam>
    /// <param name="input">The input data.</param>
    /// <returns>The result of the decision evaluation.</returns>
    public static dynamic Evaluate<TDecision>(object input) where TDecision : new()
    {
        var instance = new TDecision();
        if (instance is IDecision decision)
        {
            return decision.Evaluate(input);
        }

        throw new InvalidOperationException($"Type {typeof(TDecision).Name} does not implement IDecision. Ensure it inherits from Decision<TInput, TOutput>.");
    }
}
