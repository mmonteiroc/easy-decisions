<div align="center">
  <h1>🎯 EasyDecisions</h1>
  <p><strong>A fluent, strongly-typed, DMN-like decision engine for .NET</strong></p>

[![main](https://github.com/mmonteiroc/easy-decisions/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/mmonteiroc/easy-decisions/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/EasyDecisions.svg)](https://www.nuget.org/packages/EasyDecisions)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
</div>

<br />

## 💡 What is EasyDecisions?

**EasyDecisions** is a lightweight .NET library that brings the power and clarity of **Decision Model and Notation (DMN)** tables directly into your C# codebase. 

Instead of writing massive, deeply nested `if/else` or `switch` statements, EasyDecisions allows you to define complex business rules in a highly readable, fluent, and strongly-typed manner. It isolates your business logic from your application logic, making your code infinitely easier to read, test, and maintain.

## ❓ When should I use it?

You should use EasyDecisions whenever you find yourself writing:
- Complex mapping logic based on multiple input factors.
- Pricing engines, discount calculators, or risk assessments.
- State machines or status color mappers.
- "Spaghetti code" of massive `switch` statements or nested `if/else if` blocks.

**If you can imagine your business rule as a spreadsheet table (DMN), you should be using EasyDecisions.**

## 🚀 Installation

Install via the .NET CLI:
```bash
dotnet add package EasyDecisions
```

Or via the NuGet Package Manager Console:
```powershell
Install-Package EasyDecisions
```

## 🛠️ Usage

Using EasyDecisions is incredibly simple. It leverages C#'s native generic typing to provide full IntelliSense and compile-time safety for any data type you throw at it.

### 1. Define your Input and Output models
These can be *anything*. Classes, Records, Structs, or even primitives!

```csharp
public class UserContext
{
    public int Age { get; set; }
    public bool IsPremiumMember { get; set; }
}

// Note: Your output must have an empty constructor!
public class DiscountResult
{
    public decimal DiscountPercentage { get; set; }
    public string? Reason { get; set; }
}
```

### 2. Create your Decision Fabricator
Use the `[DecisionFabricator]` attribute to register your decision globally. 

```csharp
using EasyDecisions;

[DecisionFabricator("DISCOUNT_CALCULATOR")]
public class DiscountDecision : IDecisionFabricator<UserContext, DiscountResult>
{
    public Decision<UserContext, DiscountResult> Create()
    {
        var decision = new Decision<UserContext, DiscountResult>("DISCOUNT_CALCULATOR");
        
        // Rule 1: Young premium members
        decision.When(x => x.Age < 25)
                .And(x => x.IsPremiumMember)
                .Then(x => x.DiscountPercentage = 15m)
                .And(x => x.Reason = "Youth Premium Deal")
                .Build();
                
        // Rule 2: Adult premium members
        decision.When(x => x.Age >= 25)
                .And(x => x.IsPremiumMember)
                .Then(x => x.DiscountPercentage = 10m)
                .And(x => x.Reason = "Standard Premium Deal")
                .Build();
                
        return decision;
    }
}
```

### 3. Evaluate anywhere in your app!
Use the `DecisionFactory` to grab your pre-configured decision anywhere in your application and evaluate it against live data.

```csharp
// Get the decision engine
var calculator = DecisionFactory.Create<UserContext, DiscountResult>("DISCOUNT_CALCULATOR");

// Evaluate it against real data
var result = calculator.Evaluate(new UserContext 
{ 
    Age = 22, 
    IsPremiumMember = true 
});

Console.WriteLine($"Discount: {result.DiscountPercentage}% - {result.Reason}");
// Output: Discount: 15% - Youth Premium Deal
```

## ✨ Features
- **100% Strongly Typed**: Catch errors at compile-time, not run-time.
- **Any Data Type**: Supports classes, records, dictionaries, and primitive types.
- **Zero-Config Discovery**: Automatic assembly scanning using `DecisionFactory` & `[DecisionFabricator]`.
- **Lightweight**: Zero external dependencies. Fast and efficient.

## 🤝 Contributing
Pull requests are welcome! For major changes, please open an issue first to discuss what you would like to change. 

## 📝 License
[MIT](https://choosealicense.com/licenses/mit/)
