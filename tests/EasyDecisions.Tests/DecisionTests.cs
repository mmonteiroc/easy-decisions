using System;
using Xunit;
using EasyDecisions;
using EasyDecisions.Tests.TestModels;

namespace EasyDecisions.Tests;

public class DecisionTests
{
    [Fact]
    public void Decision_Name_IsSetCorrectly()
    {
        var d = new Decision<MyInput, MyOutput>("MyDecisionName");
        Assert.Equal("MyDecisionName", d.Name);
    }

    [Fact]
    public void Decision_Evaluate_NoRules_ReturnsDefaultOutput()
    {
        var d = new Decision<MyInput, MyOutput>("NoRules");
        var output = d.Evaluate(new MyInput { Count = 10 });
        
        Assert.Null(output.A);
        Assert.Null(output.B);
        Assert.Equal(0, output.TotalScore);
        Assert.False(output.IsApproved);
    }

    [Fact]
    public void Decision_Evaluate_ShouldApplyMatchingRules()
    {
        var d = new Decision<MyInput, MyOutput>("MyDecision");

        d.When(x => x.IsValid)
         .And(x => x.Count > 5)
         .Then(x => x.A = "hello")
         .And(x => x.B = "bye")
         .Build();

        d.When(x => x.IsValid)
         .And(x => x.Count <= 5)
         .Then(x => x.A = "hi")
         .And(x => x.B = "see ya")
         .Build();

        var inputs = new MyInput { IsValid = true, Count = 10 };
        var output = d.Evaluate(inputs);

        Assert.Equal("hello", output.A);
        Assert.Equal("bye", output.B);
    }

    [Fact]
    public void Decision_Evaluate_ShouldNotApplyNonMatchingRules()
    {
        var d = new Decision<MyInput, MyOutput>("MyDecision");

        d.When(x => x.IsValid)
         .And(x => x.Count > 5)
         .Then(x => x.A = "hello")
         .And(x => x.B = "bye")
         .Build();

        var inputs = new MyInput { IsValid = false, Count = 10 };
        var output = d.Evaluate(inputs);

        Assert.Null(output.A);
        Assert.Null(output.B);
    }

    [Fact]
    public void Decision_Evaluate_MultipleRulesMatching_ShouldApplyAllInOrder()
    {
        var d = new Decision<MyInput, MyOutput>("MyDecision");

        d.When(x => x.IsValid)
         .Then(x => x.TotalScore += 10)
         .Build();

        d.When(x => x.Count > 5)
         .Then(x => x.TotalScore += 20)
         .And(x => x.IsApproved = true)
         .Build();

        d.When(x => x.Category == "VIP")
         .Then(x => x.TotalScore *= 2)
         .Build();

        var inputs = new MyInput { IsValid = true, Count = 10, Category = "VIP" };
        var output = d.Evaluate(inputs);

        Assert.Equal(60, output.TotalScore);
        Assert.True(output.IsApproved);
    }

    [Fact]
    public void Decision_Evaluate_RuleWithNoConditions_ShouldAlwaysMatch()
    {
        var d = new Decision<MyInput, MyOutput>("MyDecision");

        d.When(x => true).Then(x => x.A = "Default").Build();

        var output = d.Evaluate(new MyInput { IsValid = false });
        Assert.Equal("Default", output.A);
    }

    [Fact]
    public void Decision_Evaluate_RuleWithNoActions_ShouldNotModifyOutput()
    {
        var d = new Decision<MyInput, MyOutput>("MyDecision");

        d.When(x => x.IsValid).Then(x => { }).Build(); // Empty action

        var output = d.Evaluate(new MyInput { IsValid = true });
        Assert.Null(output.A);
        Assert.Equal(0, output.TotalScore);
    }

    [Fact]
    public void Decision_Evaluate_ComplexConditionsAndActions()
    {
        var d = new Decision<MyInput, MyOutput>("Complex");

        d.When(x => x.IsValid)
         .And(x => x.Amount > 1000)
         .And(x => x.Category == "Premium")
         .Then(x => x.Discount = 0.20)
         .And(x => x.IsApproved = true)
         .Build();

        d.When(x => x.IsValid)
         .And(x => x.Amount > 500)
         .And(x => x.Amount <= 1000)
         .And(x => x.Category == "Standard")
         .Then(x => x.Discount = 0.10)
         .And(x => x.IsApproved = true)
         .Build();

        d.When(x => !x.IsValid)
         .Then(x => x.IsApproved = false)
         .And(x => x.TotalScore = -100)
         .Build();

        // Test 1: Premium high amount
        var out1 = d.Evaluate(new MyInput { IsValid = true, Amount = 1500, Category = "Premium" });
        Assert.True(out1.IsApproved);
        Assert.Equal(0.20, out1.Discount);

        // Test 2: Standard medium amount
        var out2 = d.Evaluate(new MyInput { IsValid = true, Amount = 750, Category = "Standard" });
        Assert.True(out2.IsApproved);
        Assert.Equal(0.10, out2.Discount);

        // Test 3: Invalid
        var out3 = d.Evaluate(new MyInput { IsValid = false, Amount = 2000, Category = "Premium" });
        Assert.False(out3.IsApproved);
        Assert.Equal(-100, out3.TotalScore);
    }
}
