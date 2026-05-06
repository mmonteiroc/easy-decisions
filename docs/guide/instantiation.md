# Instantiation & Evaluation

EasyDecisions is designed for simplicity. By keeping your business rules in dedicated classes, you gain the benefits of encapsulation and reusability.

## 1. Define your Decision

Inheriting from `Decision<TInput, TOutput>` is the standard way to define your business rules. 

```csharp
public class PricingDecision : Decision<ProductRequest, PriceResult>
{
    public PricingDecision()
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

## 2. Evaluate (The "One Way")

To keep your code clean and consistent, EasyDecisions enforces evaluation through a single, type-safe static helper. You don't need to manually instantiate or manage the decision class.

### Evaluate One-Liner

Simply call `EasyDecision.Evaluate<TDecision>(input)`. The library handles the instantiation internally, and type inference ensures the input and result are correctly typed.

```csharp
// The result variable is automatically typed to PriceResult
var result = EasyDecision.Evaluate<PricingDecision>(request);
```

> [!TIP]
> This pattern ensures that your business logic is decoupled from its execution, making your application code much cleaner and easier to read.

