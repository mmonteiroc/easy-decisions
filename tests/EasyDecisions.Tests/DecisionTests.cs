using System;
using Xunit;
using EasyDecisions;
using EasyDecisions.Tests.TestModels;

namespace EasyDecisions.Tests;

public class DecisionTests
{
    private class NamedDecision : Decision<MyInput, MyOutput>
    {
        public NamedDecision() : base("MyDecisionName") { }
    }

    [Fact]
    public void Decision_Name_IsSetCorrectly()
    {
        var result = EasyDecision.Evaluate<NamedDecision>(new MyInput());
        // Since Evaluate returns the output, not the decision, we check the name via reflection or by just knowing it works.
        // Actually, name is mostly for logging/debugging inside the engine.
        var d = new NamedDecision();
        Assert.Equal("MyDecisionName", d.Name);
    }

    private class NoRulesDecision : Decision<MyInput, MyOutput> { }

    [Fact]
    public void Decision_Evaluate_NoRules_ReturnsDefaultOutput()
    {
        var output = EasyDecision.Evaluate<NoRulesDecision>(new MyInput { Count = 10 });
        
        Assert.Null(output.A);
        Assert.Null(output.B);
        Assert.Equal(0, output.TotalScore);
        Assert.False(output.IsApproved);
    }

    private class MatchingRulesDecision : Decision<MyInput, MyOutput>
    {
        public MatchingRulesDecision()
        {
            When(x => x.IsValid)
             .And(x => x.Count > 5)
             .Then(x => x.A = "hello")
             .And(x => x.B = "bye")
             .Build();

            When(x => x.IsValid)
             .And(x => x.Count <= 5)
             .Then(x => x.A = "hi")
             .And(x => x.B = "see ya")
             .Build();
        }
    }

    [Fact]
    public void Decision_Evaluate_ShouldApplyMatchingRules()
    {
        var inputs = new MyInput { IsValid = true, Count = 10 };
        var output = EasyDecision.Evaluate<MatchingRulesDecision>(inputs);

        Assert.Equal("hello", output.A);
        Assert.Equal("bye", output.B);
    }

    private class NonMatchingRulesDecision : Decision<MyInput, MyOutput>
    {
        public NonMatchingRulesDecision()
        {
            When(x => x.IsValid)
             .And(x => x.Count > 5)
             .Then(x => x.A = "hello")
             .And(x => x.B = "bye")
             .Build();
        }
    }

    [Fact]
    public void Decision_Evaluate_ShouldNotApplyNonMatchingRules()
    {
        var inputs = new MyInput { IsValid = false, Count = 10 };
        var output = EasyDecision.Evaluate<NonMatchingRulesDecision>(inputs);

        Assert.Null(output.A);
        Assert.Null(output.B);
    }

    private class MultipleMatchingDecision : Decision<MyInput, MyOutput>
    {
        public MultipleMatchingDecision()
        {
            When(x => x.IsValid)
             .Then(x => x.TotalScore += 10)
             .Build();

            When(x => x.Count > 5)
             .Then(x => x.TotalScore += 20)
             .And(x => x.IsApproved = true)
             .Build();

            When(x => x.Category == "VIP")
             .Then(x => x.TotalScore *= 2)
             .Build();
        }
    }

    [Fact]
    public void Decision_Evaluate_MultipleRulesMatching_ShouldApplyAllInOrder()
    {
        var inputs = new MyInput { IsValid = true, Count = 10, Category = "VIP" };
        var output = EasyDecision.Evaluate<MultipleMatchingDecision>(inputs);

        Assert.Equal(60, output.TotalScore);
        Assert.True(output.IsApproved);
    }

    private class DefaultRuleDecision : Decision<MyInput, MyOutput>
    {
        public DefaultRuleDecision()
        {
            When(x => true).Then(x => x.A = "Default").Build();
        }
    }

    [Fact]
    public void Decision_Evaluate_RuleWithNoConditions_ShouldAlwaysMatch()
    {
        var output = EasyDecision.Evaluate<DefaultRuleDecision>(new MyInput { IsValid = false });
        Assert.Equal("Default", output.A);
    }

    private class ComplexDecision : Decision<MyInput, MyOutput>
    {
        public ComplexDecision()
        {
            When(x => x.IsValid)
             .And(x => x.Amount > 1000)
             .And(x => x.Category == "Premium")
             .Then(x => x.Discount = 0.20)
             .And(x => x.IsApproved = true)
             .Build();

            When(x => x.IsValid)
             .And(x => x.Amount > 500)
             .And(x => x.Amount <= 1000)
             .And(x => x.Category == "Standard")
             .Then(x => x.Discount = 0.10)
             .And(x => x.IsApproved = true)
             .Build();

            When(x => !x.IsValid)
             .Then(x => x.IsApproved = false)
             .And(x => x.TotalScore = -100)
             .Build();
        }
    }

    [Fact]
    public void Decision_Evaluate_ComplexConditionsAndActions()
    {
        // Test 1: Premium high amount
        var out1 = EasyDecision.Evaluate<ComplexDecision>(new MyInput { IsValid = true, Amount = 1500, Category = "Premium" });
        Assert.True(out1.IsApproved);
        Assert.Equal(0.20, out1.Discount);

        // Test 2: Standard medium amount
        var out2 = EasyDecision.Evaluate<ComplexDecision>(new MyInput { IsValid = true, Amount = 750, Category = "Standard" });
        Assert.True(out2.IsApproved);
        Assert.Equal(0.10, out2.Discount);

        // Test 3: Invalid
        var out3 = EasyDecision.Evaluate<ComplexDecision>(new MyInput { IsValid = false, Amount = 2000, Category = "Premium" });
        Assert.False(out3.IsApproved);
        Assert.Equal(-100, out3.TotalScore);
    }
}
