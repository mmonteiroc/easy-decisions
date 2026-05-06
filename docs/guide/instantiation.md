# Instantiation & Evaluation

EasyDecisions provides several ways to define, instantiate, and evaluate decisions, allowing you to choose the pattern that best fits your application's architecture.

## 1. Class Derivation (Recommended)

Inheriting from `Decision<TInput, TOutput>` is the most robust and type-safe way to define your business rules. It keeps the rules encapsulated within a single class.

### Definition
```csharp
public class PricingDecision : Decision<ProductRequest, PriceResult>
{
    public PricingDecision() : base("ProductPricing")
    {
        HitPolicy = HitPolicy.First;

        When(p => p.Category == "Electronics")
            .Then(r => r.Price = p.BasePrice * 0.9m)
            .Build();

        // Default rule
        When(_ => true)
            .Then(r => r.Price = p.BasePrice)
            .Build();
    }
}
```

### Usage
```csharp
var decision = new PricingDecision();
var result = decision.Evaluate(request);
```

---

## 2. Type-Safe Helper (`EasyDecision`)

The `EasyDecision` static class provides a convenient way to evaluate decisions without manually managing instances.

### Evaluate One-Liner
If your decision has a parameterless constructor (like the one above), you can evaluate it in one line:

```csharp
var result = EasyDecision.Evaluate<PricingDecision, ProductRequest, PriceResult>(request);
```

### Create Instance
You can also use it to create an instance of your decision type:
```csharp
var decision = EasyDecision.Create<PricingDecision>();
```

---

## 3. Zero-Config Discovery (Factory Pattern)

For scenarios where you want to decouple the caller from the specific decision implementation, you can use the `DecisionFactory`. This uses assembly scanning to find and register decisions automatically.

### Registration
Decorate a class with the `[Decision]` attribute and implement `IDecisionFactory<TInput, TOutput>`.

```csharp
[Decision("PREMIUM_PRICING")]
public class PremiumPricingFactory : IDecisionFactory<ProductRequest, PriceResult>
{
    public Decision<ProductRequest, PriceResult> Create()
    {
        return new PricingDecision().UsingHitPolicy(HitPolicy.Unique);
    }
}
```

### Usage
You can then retrieve the decision by its name from anywhere in your app:

```csharp
var decision = DecisionFactory.Create<ProductRequest, PriceResult>("PREMIUM_PRICING");
var result = decision.Evaluate(request);
```

If there is only **one** factory registered for a specific input/output pair, you can even omit the name:

```csharp
var decision = DecisionFactory.Create<ProductRequest, PriceResult>();
```

---

## 4. Ad-hoc (Inline) Definition

While not recommended for complex logic, you can define decisions inline for simple mapping or one-off logic.

```csharp
var decision = new Decision<int, string>("AgeMapper")
    .When(age => age < 18).Then(o => o.Value = "Minor").Build();

var result = decision.Evaluate(20);
```
