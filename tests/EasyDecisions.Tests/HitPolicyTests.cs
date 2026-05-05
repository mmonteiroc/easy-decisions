using System;
using Xunit;
using EasyDecisions;
using EasyDecisions.Tests.TestModels;

namespace EasyDecisions.Tests;

public class HitPolicyTests
{
    [Fact]
    public void HitPolicy_Default_IsCollect()
    {
        var d = new Decision<MyInput, MyOutput>("DefaultCollect");
        Assert.Equal(HitPolicy.Collect, d.HitPolicy);
    }

    [Fact]
    public void HitPolicy_First_ShouldOnlyApplyFirstMatchingRule()
    {
        var d = new Decision<MyInput, MyOutput>("FirstPolicy")
            .UsingHitPolicy(HitPolicy.First);

        d.When(x => x.Count > 5)
         .Then(x => x.TotalScore = 10)
         .Build();

        d.When(x => x.Count > 0)
         .Then(x => x.TotalScore = 20)
         .Build();

        var output = d.Evaluate(new MyInput { Count = 10 });

        // Only the first rule (TotalScore = 10) should be applied
        Assert.Equal(10, output.TotalScore);
    }

    [Fact]
    public void HitPolicy_Unique_ShouldApplyWhenExactlyOneMatches()
    {
        var d = new Decision<MyInput, MyOutput>("UniquePolicy")
            .UsingHitPolicy(HitPolicy.Unique);

        d.When(x => x.Count > 5)
         .Then(x => x.TotalScore = 10)
         .Build();

        d.When(x => x.Count <= 5)
         .Then(x => x.TotalScore = 20)
         .Build();

        var output = d.Evaluate(new MyInput { Count = 10 });

        Assert.Equal(10, output.TotalScore);
    }

    [Fact]
    public void HitPolicy_Unique_ShouldThrowWhenMoreThanOneMatches()
    {
        var d = new Decision<MyInput, MyOutput>("UniquePolicyError")
            .UsingHitPolicy(HitPolicy.Unique);

        d.When(x => x.Count > 5)
         .Then(x => x.TotalScore = 10)
         .Build();

        d.When(x => x.Count > 0)
         .Then(x => x.TotalScore = 20)
         .Build();

        // Both rules match for Count = 10
        var ex = Assert.Throws<InvalidOperationException>(() => d.Evaluate(new MyInput { Count = 10 }));
        Assert.Contains("hit policy is 'Unique', but 2 rules matched", ex.Message);
    }

    [Fact]
    public void HitPolicy_Unique_ShouldReturnDefaultWhenNoneMatch()
    {
        var d = new Decision<MyInput, MyOutput>("UniquePolicyNone")
            .UsingHitPolicy(HitPolicy.Unique);

        d.When(x => x.Count > 100)
         .Then(x => x.TotalScore = 10)
         .Build();

        var output = d.Evaluate(new MyInput { Count = 10 });

        Assert.Equal(0, output.TotalScore);
    }

    [Fact]
    public void HitPolicy_Collect_ShouldApplyAllMatches()
    {
        var d = new Decision<MyInput, MyOutput>("CollectPolicy")
            .UsingHitPolicy(HitPolicy.Collect);

        d.When(x => x.Count > 5)
         .Then(x => x.TotalScore += 10)
         .Build();

        d.When(x => x.Count > 0)
         .Then(x => x.TotalScore += 20)
         .Build();

        var output = d.Evaluate(new MyInput { Count = 10 });

        // Both rules applied: 10 + 20 = 30
        Assert.Equal(30, output.TotalScore);
    }
}
