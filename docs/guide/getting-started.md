# Getting Started

## Installation

Install the package via NuGet:

```bash
dotnet add package EasyDecisions
```

## Basic Example

Let's define a simple decision that determines if a user is eligible for a discount based on their cart total and membership status.

### 1. Define your Input and Output

```csharp
public record CartInput(decimal Total, bool IsPremiumMember);
public record DiscountOutput(decimal DiscountPercentage);
```

### 2. Build the Decision

The recommended way to define rules is by inheriting from `Decision<TInput, TOutput>`. This keeps your business logic clean and isolated.

```csharp
using EasyDecisions;

public class DiscountDecision : Decision<CartInput, DiscountOutput>
{
    public DiscountDecision() : base("CartDiscount")
    {
        // Premium members spending over $100 get 20% off
        When(i => i.IsPremiumMember && i.Total > 100)
            .Then(o => o.DiscountPercentage = 20m)
            .Build();

        // Regular members spending over $100 get 10% off
        When(i => !i.IsPremiumMember && i.Total > 100)
            .Then(o => o.DiscountPercentage = 10m)
            .Build();
    }
}
```

### 3. Evaluate

You can evaluate your decision by simply instantiating it:

```csharp
var input = new CartInput(150, true);
var result = new DiscountDecision().Evaluate(input);

Console.WriteLine($"Discount: {result.DiscountPercentage}%"); // Output: Discount: 20%
```

Or using the `EasyDecision` helper for a more fluent one-liner:

```csharp
var result = EasyDecision.Evaluate<DiscountDecision, CartInput, DiscountOutput>(input);
```

With just a few lines of code, you have a fully functional, type-safe decision engine!
