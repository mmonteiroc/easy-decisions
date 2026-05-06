# FEEL-like Helpers

EasyDecisions provides a set of extension methods that mimic the syntax of **FEEL** (Friendly Enough Expression Language), the expression language used in DMN (Decision Model and Notation).

These helpers make your C# decision rules more readable and closer to "Business Language".

## Why use them?

In standard C#, range checks and list checks can become verbose:

```csharp
// Standard C#
.When(x => x.Age >= 18 && x.Age <= 65)
.When(x => x.Status == "Active" || x.Status == "Pending")
```

With **FEEL-like Helpers**, these become much more expressive:

```csharp
using EasyDecisions.Extensions;

// Expressive business rules
.When(x => x.Age.IsBetween(18, 65))
.When(x => x.Status.IsIn("Active", "Pending"))
```

## Available Helpers

### Range Checks

| Method | FEEL Equivalent | Description |
|--------|-----------------|-------------|
| `IsBetween(min, max)` | `[min..max]` | Inclusive range check. |
| `IsBetweenExclusive(min, max)` | `(min..max)` | Exclusive range check. |
| `IsBetween(min, max, incMin, incMax)` | Mixed | Configurable inclusivity (e.g., `[min..max)`). |
| `IsAtLeast(min)` | `>= min` | Greater than or equal to. |
| `IsAtMost(max)` | `<= max` | Less than or equal to. |
| `IsGreaterThan(min)` | `> min` | Strictly greater than. |
| `IsLessThan(max)` | `< max` | Strictly less than. |

#### Examples

```csharp
// Age between 18 and 65 (inclusive)
x => x.Age.IsBetween(18, 65)

// Score strictly between 0 and 100
x => x.Score.IsBetweenExclusive(0, 100)

// Age is at least 18
x => x.Age.IsAtLeast(18)
```

### Collection Checks

| Method | FEEL Equivalent | Description |
|--------|-----------------|-------------|
| `IsIn(params items)` | `x in (a, b, c)` | Checks if the value is in the list. |
| `IsIn(IEnumerable)` | `x in list` | Checks if the value is in a collection. |
| `IsNotIn(params items)` | `not(x in (a, b, c))` | Checks if the value is NOT in the list. |

#### Examples

```csharp
// Category is one of the allowed values
x => x.Category.IsIn("Electronics", "Books", "Toys")

// Status is NOT 'Expired' or 'Cancelled'
x => x.Status.IsNotIn("Expired", "Cancelled")
```

## Supported Types

These helpers work on any type that implements `IComparable<T>`, including:
- `int`, `long`, `decimal`, `double`, `float`
- `string`
- `DateTime`, `DateTimeOffset`
- `TimeSpan`
- Custom types implementing `IComparable<T>`
