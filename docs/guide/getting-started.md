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

```csharp
using EasyDecisions;

var discountDecision = Decision<CartInput, DiscountOutput>.Builder()
    // Premium members spending over $100 get 20% off
    .When(i => i.IsPremiumMember).And(i => i.Total > 100)
    .Then(new DiscountOutput(20m))
    
    // Regular members spending over $100 get 10% off
    .When(i => !i.IsPremiumMember).And(i => i.Total > 100)
    .Then(new DiscountOutput(10m))
    
    // Default outcome if no rules match
    .DefaultTo(new DiscountOutput(0m))
    .Build();
```

### 3. Evaluate

```csharp
var input = new CartInput(150, true);
var result = discountDecision.Evaluate(input);

Console.WriteLine($"Discount: {result.DiscountPercentage}%"); // Output: Discount: 20%
```

With just a few lines of code, you have a fully functional, type-safe decision engine!
