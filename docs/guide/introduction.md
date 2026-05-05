# What is EasyDecisions?

`EasyDecisions` is a lightweight, fluent decision engine for .NET, built as an alternative to complex Decision Model and Notation (DMN) engines like Camunda.

## The Pain Points of DMN

Traditional DMN engines force you to define your business rules in external XML files. While powerful, this approach has significant drawbacks:

- **No Compile-Time Safety:** Typos in variable names aren't caught until your app runs.
- **Hard to Refactor:** Renaming a property in C# breaks the DMN file unless manually updated.
- **Context Switching:** Developers have to constantly switch between C# code and visual XML editors.
- **Testing Complexity:** Testing requires loading external files and evaluating string-based inputs.

## The EasyDecisions Solution

`EasyDecisions` brings the power of decision tables directly into your C# codebase.

Instead of writing XML, you write this:

```csharp
var decision = new Decision<MyInput, MyOutput>("Eligibility");

decision.When(i => i.Age >= 18).And(i => i.Country == "US")
    .Then(o => { o.CanDrink = true; o.CanVote = true; })
    .Build();
```

By keeping your rules in code, you gain **IntelliSense**, **compile-time validation**, and seamless **refactoring**, while still keeping your business rules clean and readable.
