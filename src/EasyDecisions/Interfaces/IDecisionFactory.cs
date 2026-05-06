using System;

namespace EasyDecisions;

/// <summary>
/// Defines a factory class for a specific decision.
/// </summary>
/// <typeparam name="TInput">The input type of the decision.</typeparam>
/// <typeparam name="TOutput">The output type of the decision.</typeparam>
public interface IDecisionFactory<TInput, TOutput> where TOutput : new()
{
    /// <summary>
    /// Creates and returns the decision instance.
    /// </summary>
    Decision<TInput, TOutput> Create();
}

/// <summary>
/// Obsolete. Use <see cref="IDecisionFactory{TInput, TOutput}"/> instead.
/// </summary>
[Obsolete("Use IDecisionFactory<TInput, TOutput> instead.")]
public interface IDecisionFabricator<TInput, TOutput> : IDecisionFactory<TInput, TOutput> where TOutput : new()
{
}
