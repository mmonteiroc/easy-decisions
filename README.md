<div align="center">
  <h1>🎯 EasyDecisions</h1>
  <p><strong>A fluent, strongly-typed, DMN-like decision engine for .NET</strong></p>
  <p><a href="https://mmonteiroc.github.io/easy-decisions/"><strong>Explore the Docs »</strong></a></p>

[![main](https://github.com/mmonteiroc/easy-decisions/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/mmonteiroc/easy-decisions/actions/workflows/ci.yml)
[![SonarQube](https://github.com/mmonteiroc/easy-decisions/actions/workflows/sonar.yml/badge.svg?branch=main)](https://github.com/mmonteiroc/easy-decisions/actions/workflows/sonar.yml)
[![NuGet](https://img.shields.io/nuget/v/EasyDecisions.svg)](https://www.nuget.org/packages/EasyDecisions)
[![GitHub stars](https://img.shields.io/github/stars/mmonteiroc/easy-decisions)](https://github.com/mmonteiroc/easy-decisions/stargazers)
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

EasyDecisions offers multiple ways to define and evaluate your business rules, ranging from simple ad-hoc tables to strongly-typed class derivations and automatic factory discovery.

### 1. Define your Models
Inputs and Outputs can be any C# type (classes, records, or primitives). Your output must have a parameterless constructor.

```csharp
public record UserContext(int Age, bool IsPremiumMember);
public class DiscountResult { public decimal Percentage { get; set; } }
```

### 2. Define your Decision
The recommended way is to inherit from `Decision<TInput, TOutput>`. This keeps your logic encapsulated and reusable. Rules are automatically registered, so no need for a final `.Build()` call.

```csharp
public class DiscountDecision : Decision<UserContext, DiscountResult>
{
    public DiscountDecision() : base("DISCOUNT_CALC")
    {
        HitPolicy = HitPolicy.First;

        // You can define multiple rows (rules) in your decision table
        When(x => x.Age < 25 && x.IsPremiumMember)
            .Then()
            .Set(x => x.Percentage = 15m);

        When(x => x.IsPremiumMember)
            .Then()
            .Set(x => x.Percentage = 10m);
            
        // Default rule if nothing else matches
        When(_ => true)
            .Then()
            .Set(x => x.Percentage = 0m);
    }
}
```

### 3. Evaluate
The **only** way to evaluate a decision is through the `EasyDecision.Evaluate` helper. It finds and evaluates your decision in a single, type-safe line.

```csharp
// The input type is inferred, and the result is type-safe
var result = EasyDecision.Evaluate<DiscountDecision>(userContext);
```


## 🚦 Hit Policies

EasyDecisions supports common DMN hit policies to control how multiple matching rules are handled. You can set the policy using `.UsingHitPolicy(HitPolicy.Name)`.

### 1. Collect (Default)
Evaluates **all** rules. Every matching rule is applied to the output object in the order they were defined.
```csharp
decision.UsingHitPolicy(HitPolicy.Collect);
// If Rule A and Rule B match, both are applied.
```

### 2. First
Returns the result of the **first** matching rule only. Evaluation stops immediately after a match is found.
```csharp
decision.UsingHitPolicy(HitPolicy.First);
// Useful for priority-based rules (e.g., specific case before general case).
```

### 3. Unique
Ensures that **exactly one** rule matches. If multiple rules match the input, an `InvalidOperationException` is thrown.
```csharp
decision.UsingHitPolicy(HitPolicy.Unique);
// The safest policy for mutually exclusive logic.
```

## ✨ Features
- **100% Strongly Typed**: Catch errors at compile-time, not run-time.
- **Any Data Type**: Supports classes, records, dictionaries, and primitive types.
- **Zero-Config Discovery**: Automatic assembly scanning using `DecisionFactory` & `[Decision]`.
- **Lightweight**: Zero external dependencies. Fast and efficient.

## 🔧 Troubleshooting

### Do I need to register decisions in my Dependency Injection (DI) container?
**No.** EasyDecisions requires essentially zero setup. It uses a static `DecisionFactory` that automatically scans your loaded assemblies the very first time you call `DecisionFactory.Create()`. It finds any classes decorated with the `[Decision]` attribute and registers them internally.

### What if it says "No decision factory found" but I added the attribute?
In rare cases, if your Decision Factories are located in a completely separate project or assembly that hasn't been executed or loaded into the `AppDomain` yet, the automatic scanner might miss it. 

You can fix this by explicitly registering that assembly anywhere in your application startup (like in your `Program.cs`):

```csharp
// Register the assembly containing your custom decisions
DecisionFactory.RegisterAssembly(typeof(MyCustomDecision).Assembly);
```

## 🤝 Contributing
Pull requests are welcome! For major changes, please open an issue first to discuss what you would like to change. 

## 📝 License
[MIT](https://choosealicense.com/licenses/mit/)
