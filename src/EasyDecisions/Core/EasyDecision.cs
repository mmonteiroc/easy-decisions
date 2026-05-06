using System;
using System.Reflection;

namespace EasyDecisions;

/// <summary>
/// A helper class to evaluate decisions fluently without needing to specify string IDs or full generic types.
/// </summary>
/// <typeparam name="TDecision">The type of the decision class to instantiate.</typeparam>
public static class EasyDecision<TDecision> where TDecision : class, new()
{
    private static readonly TDecision _instance = new();

    /// <summary>
    /// Evaluates the decision using the provided input. 
    /// Note: Returns dynamic because the return type cannot be inferred strictly at compile time 
    /// from just the decision type without additional generic arguments.
    /// </summary>
    /// <param name="input">The input to the decision.</param>
    /// <returns>The result of the decision evaluation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the decision class does not inherit from Decision&lt;TInput, TOutput&gt;.</exception>
    public static dynamic Evaluate(dynamic input)
    {
        var type = typeof(TDecision);
        var baseType = type.BaseType;

        while (baseType != null)
        {
            if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(Decision<,>))
            {
                var evaluateMethod = baseType.GetMethod("Evaluate", BindingFlags.Public | BindingFlags.Instance);
                if (evaluateMethod != null)
                {
                    return evaluateMethod.Invoke(_instance, new object[] { input })!;
                }
            }
            baseType = baseType.BaseType;
        }

        throw new InvalidOperationException($"Type {type.Name} does not inherit from Decision<TInput, TOutput>.");
    }
}

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
