using System;
using System.Collections.Generic;

namespace EasyDecisions;

/// <summary>
/// Represents a decision table, similar to a DMN (Decision Model and Notation) file.
/// Allows defining rules based on inputs of type <typeparamref name="TInput"/> to produce an output of type <typeparamref name="TOutput"/>.
/// </summary>
/// <typeparam name="TInput">The type of the input data.</typeparam>
/// <typeparam name="TOutput">The type of the output data, which must have a parameterless constructor.</typeparam>
public class Decision<TInput, TOutput> where TOutput : new()
{
    public string Name { get; }
    private readonly List<DecisionRule<TInput, TOutput>> _rules = new();

    public Decision(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Starts the definition of a new decision rule with a condition.
    /// </summary>
    /// <param name="predicate">The condition that must be met for this rule to apply.</param>
    /// <returns>A rule builder to chain further conditions or actions.</returns>
    public RuleBuilder<TInput, TOutput> When(Func<TInput, bool> predicate)
    {
        return new RuleBuilder<TInput, TOutput>(this).And(predicate);
    }

    internal void AddRule(DecisionRule<TInput, TOutput> rule)
    {
        _rules.Add(rule);
    }

    /// <summary>
    /// Evaluates the decision against the provided input and returns the resulting output.
    /// Rules are evaluated in the order they were added.
    /// </summary>
    /// <param name="input">The input data to evaluate.</param>
    /// <returns>The resulting output after applying all matching rules.</returns>
    public TOutput Evaluate(TInput input)
    {
        var output = new TOutput();

        foreach (var rule in _rules)
        {
            if (rule.Matches(input))
            {
                rule.Apply(output);
            }
        }

        return output;
    }
}

/// <summary>
/// Represents a single rule within a decision table.
/// </summary>
public class DecisionRule<TInput, TOutput>
{
    private readonly List<Func<TInput, bool>> _conditions = new();
    private readonly List<Action<TOutput>> _actions = new();

    internal void AddCondition(Func<TInput, bool> condition)
    {
        _conditions.Add(condition);
    }

    internal void AddAction(Action<TOutput> action)
    {
        _actions.Add(action);
    }

    internal bool Matches(TInput input)
    {
        // If there are no conditions, it could be considered a default rule
        if (_conditions.Count == 0) return true;

        foreach (var condition in _conditions)
        {
            if (!condition(input))
            {
                return false;
            }
        }
        return true;
    }

    internal void Apply(TOutput output)
    {
        foreach (var action in _actions)
        {
            action(output);
        }
    }
}

/// <summary>
/// Builder for defining the conditions of a decision rule.
/// </summary>
public class RuleBuilder<TInput, TOutput> where TOutput : new()
{
    private readonly Decision<TInput, TOutput> _decision;
    private readonly DecisionRule<TInput, TOutput> _rule = new();

    internal RuleBuilder(Decision<TInput, TOutput> decision)
    {
        _decision = decision;
    }

    /// <summary>
    /// Adds an additional condition to the rule. All conditions must be true for the rule to match (logical AND).
    /// </summary>
    /// <param name="predicate">The additional condition.</param>
    /// <returns>The rule builder for chaining.</returns>
    public RuleBuilder<TInput, TOutput> And(Func<TInput, bool> predicate)
    {
        _rule.AddCondition(predicate);
        return this;
    }

    /// <summary>
    /// Specifies the action to take when the rule's conditions are met.
    /// </summary>
    /// <param name="action">The action to modify the output.</param>
    /// <returns>An action builder to chain further actions or complete the rule.</returns>
    public RuleActionBuilder<TInput, TOutput> Then(Action<TOutput> action)
    {
        var actionBuilder = new RuleActionBuilder<TInput, TOutput>(_decision, _rule);
        return actionBuilder.And(action);
    }
}

/// <summary>
/// Builder for defining the actions of a decision rule and completing its registration.
/// </summary>
public class RuleActionBuilder<TInput, TOutput> where TOutput : new()
{
    private readonly Decision<TInput, TOutput> _decision;
    private readonly DecisionRule<TInput, TOutput> _rule;

    internal RuleActionBuilder(Decision<TInput, TOutput> decision, DecisionRule<TInput, TOutput> rule)
    {
        _decision = decision;
        _rule = rule;
    }

    /// <summary>
    /// Adds an additional action to be executed when the rule matches.
    /// </summary>
    /// <param name="action">The additional action to modify the output.</param>
    /// <returns>The action builder for chaining.</returns>
    public RuleActionBuilder<TInput, TOutput> And(Action<TOutput> action)
    {
        _rule.AddAction(action);
        return this;
    }

    /// <summary>
    /// Finalizes the rule and registers it with the decision table.
    /// </summary>
    public void Build()
    {
        _decision.AddRule(_rule);
    }
}
