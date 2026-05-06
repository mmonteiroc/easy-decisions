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
public class EligibilityDecision : Decision<MyInput, MyOutput>
{
    public EligibilityDecision() : base("Eligibility")
    {
        When(i => i.Age >= 18 && i.Country == "US")
            .Then()
            .Set(o => o.CanDrink = true)
            .Set(o => o.CanVote = true);
    }
}
```

By keeping your rules in code, you gain **IntelliSense**, **compile-time validation**, and seamless **refactoring**, while still keeping your business rules clean and readable.

## When to use EasyDecisions?

You should consider using `EasyDecisions` when:

- You have complex mapping logic based on multiple input factors.
- You need to build pricing engines, discount calculators, or risk assessments.
- You have "spaghetti code" consisting of massive `switch` statements or nested `if/else` blocks.
- **You need to reuse the same decision** in multiple places. By encapsulating a decision (where one input generates one output) into a dedicated class, you avoid duplicating complex conditions across your codebase or creating unnecessary boilerplate dependency classes.
