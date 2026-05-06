# Strategy Selector

One of the most powerful patterns for `EasyDecisions` is using it as a **Dynamic Strategy Selector**. 

Instead of returning simple data types, you can return an **Interface**. This allows your business rules to decide which implementation of a strategy should be used at runtime, while your calling code remains completely decoupled from the specific implementations.

## The Scenario: AI Provider Selector

Imagine you have an application that integrates with multiple AI providers (OpenAI, Google, Codex). You want to select the provider based on the user's preference, but if they are a **Premium** user, you want to automatically upgrade them to the "Max" mode of that provider.

### 1. Define the Strategy Interface

First, define the interface that all AI providers will implement.

```csharp
public interface IAiApi 
{
    string Call(string prompt);
}

// Implementations
public class OpenAIService : IAiApi { ... }
public class OpenAI_MaxMode : IAiApi { ... }
public class GoogleAIService : IAiApi { ... }
public class GoogleAI_MaxMode : IAiApi { ... }
public class CodexService : IAiApi { ... }
```

### 2. Define Input and Output

The output of our decision will contain the instance of `IAiApi` that should be used.

```csharp
public record AiRequest(string PreferredVendor, bool IsPremium);

public class AiSelectionResult 
{
    // The engine will populate this interface with a concrete instance
    public IAiApi aiApi { get; set; }
}
```

### 3. Build the Decision Table

Now, create the decision class. The rules will assign concrete implementations to the `aiApi` property.

```csharp
public class AiStrategyDecision : Decision<AiRequest, AiSelectionResult>
{
    public AiStrategyDecision()
    {
        // OpenAI Rules
        When(i => i.PreferredVendor == "OpenAI" && i.IsPremium)
            .Then().Set(o => o.aiApi = new OpenAI_MaxMode());

        When(i => i.PreferredVendor == "OpenAI" && !i.IsPremium)
            .Then().Set(o => o.aiApi = new OpenAIService());

        // Google Rules
        When(i => i.PreferredVendor == "Google" && i.IsPremium)
            .Then().Set(o => o.aiApi = new GoogleAI_MaxMode());

        When(i => i.PreferredVendor == "Google" && !i.IsPremium)
            .Then().Set(o => o.aiApi = new GoogleAIService());
            
        // Codex Rules
        When(i => i.PreferredVendor == "Codex")
            .Then().Set(o => o.aiApi = new CodexService());
    }
}
```

### 4. Use the Result

The beauty of this approach is that the calling code doesn't care which model was selected. It just calls the interface method.

```csharp
var request = new AiRequest("OpenAI", IsPremium: true);

// The engine selects the "OpenAI_MaxMode" based on the rules
var result = EasyDecision.Evaluate<AiStrategyDecision>(request);

// Execute the strategy polymorphically
var response = result.aiApi.Call("Write a poem about C#");
```

## Why use this?

1. **Decoupling**: Your main application logic doesn't have `if/else` or `switch` statements to pick a strategy.
2. **Extensibility**: Adding a new AI provider (like Anthropic) only requires a new implementation of `IAiApi` and a new row in your decision table.
3. **Testability**: You can easily mock `IAiApi` and verify that the decision engine selects the correct mock based on inputs.
