# Hit Policies

Hit Policies define how to handle multiple matching rules in a decision table. They determine which rules are applied and in what order when an input matches more than one rule.

EasyDecisions supports the most commonly used DMN Hit Policies.

## Available Hit Policies

### 1. Collect (Default)

The `Collect` policy evaluates all rules in the order they were defined. Every rule that matches the input is applied to the output object.

This is the default policy and is useful when multiple rules contribute to a single result (e.g., adding up points or applying multiple tags).

```csharp
var decision = new Decision<MyInput, MyOutput>("PointsCalculation")
    .UsingHitPolicy(HitPolicy.Collect);

decision.When(i => i.IsRegistered).Then(o => o.Points += 10).Build();
decision.When(i => i.IsFirstPurchase).Then(o => o.Points += 20).Build();

// If both are true, Points will be 30.
```

> [!NOTE]
> **Who wins in "Collect"?**
> Since rules are applied in the order they were defined, if two matching rules modify the same variable (using `=` instead of `+=`), the **last** rule applied will effectively "win" by overwriting the previous value.

### 2. First

The `First` policy returns the result of the first matching rule. Evaluation stops as soon as a match is found.

This is useful when rules are ordered by priority or specificity.

```csharp
var decision = new Decision<MyInput, MyOutput>("Shipping")
    .UsingHitPolicy(HitPolicy.First);

// Specific rules first
decision.When(i => i.Country == "US" && i.State == "AK").Then(o => o.Cost = 50).Build();
decision.When(i => i.Country == "US").Then(o => o.Cost = 10).Build();

// If Country is US and State is AK, Cost will be 50 and the second rule won't even be evaluated.
```

### 3. Unique

The `Unique` policy ensures that exactly one rule matches the input. If more than one rule matches, an `InvalidOperationException` is thrown.

This is the safest policy for mutually exclusive rules, as it prevents accidental overlaps in logic.

```csharp
var decision = new Decision<MyInput, MyOutput>("Approval")
    .UsingHitPolicy(HitPolicy.Unique);

decision.When(i => i.Score > 80).Then(o => o.Approved = true).Build();
decision.When(i => i.Score <= 80).Then(o => o.Approved = false).Build();

// If a rule was added that overlaps (e.g. Score > 50), Evaluate() would throw.
```

## How to Set a Hit Policy

You can set the hit policy using the `.UsingHitPolicy()` method on the `Decision` object:

```csharp
var decision = new Decision<TInput, TOutput>("MyDecision")
    .UsingHitPolicy(HitPolicy.First);
```

If not specified, the default hit policy is `HitPolicy.Collect`.
