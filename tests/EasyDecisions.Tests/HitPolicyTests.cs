using Xunit;
using EasyDecisions.Tests.TestModels;

namespace EasyDecisions.Tests;

public class HitPolicyTests
{
    private class DefaultCollectDecision : Decision<MyInput, MyOutput> { }

    [Fact]
    public void HitPolicy_Default_IsCollect()
    {
        var d = new DefaultCollectDecision();
        Assert.Equal(HitPolicy.Collect, d.HitPolicy);
    }

    private class FirstPolicyDecision : Decision<MyInput, MyOutput>
    {
        public FirstPolicyDecision()
        {
            UsingHitPolicy(HitPolicy.First);
            When(x => x.Count > 5).Then(x => x.A = "First").Build();
            When(x => x.Count > 0).Then(x => x.A = "Second").Build();
        }
    }

    [Fact]
    public void HitPolicy_First_ShouldOnlyApplyFirstMatchingRule()
    {
        var output = EasyDecision.Evaluate<FirstPolicyDecision>(new MyInput { Count = 10 });
        Assert.Equal("First", output.A);
    }

    private class UniquePolicyDecision : Decision<MyInput, MyOutput>
    {
        public UniquePolicyDecision()
        {
            UsingHitPolicy(HitPolicy.Unique);
            When(x => x.Count > 5).Then(x => x.A = "Unique").Build();
        }
    }

    [Fact]
    public void HitPolicy_Unique_ShouldApplyWhenExactlyOneMatches()
    {
        var output = EasyDecision.Evaluate<UniquePolicyDecision>(new MyInput { Count = 10 });
        Assert.Equal("Unique", output.A);
    }

    private class UniquePolicyErrorDecision : Decision<MyInput, MyOutput>
    {
        public UniquePolicyErrorDecision()
        {
            UsingHitPolicy(HitPolicy.Unique);
            When(x => x.Count > 5).Then(x => x.A = "Match 1").Build();
            When(x => x.Count > 0).Then(x => x.A = "Match 2").Build();
        }
    }

    [Fact]
    public void HitPolicy_Unique_ShouldThrowWhenMoreThanOneMatches()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            EasyDecision.Evaluate<UniquePolicyErrorDecision>(new MyInput { Count = 10 });
        });
    }

    private class UniquePolicyNoneDecision : Decision<MyInput, MyOutput>
    {
        public UniquePolicyNoneDecision()
        {
            UsingHitPolicy(HitPolicy.Unique);
            When(x => x.Count > 100).Then(x => x.A = "None").Build();
        }
    }

    [Fact]
    public void HitPolicy_Unique_ShouldReturnDefaultWhenNoneMatch()
    {
        var output = EasyDecision.Evaluate<UniquePolicyNoneDecision>(new MyInput { Count = 10 });
        Assert.Null(output.A);
    }

    private class CollectPolicyDecision : Decision<MyInput, MyOutput>
    {
        public CollectPolicyDecision()
        {
            UsingHitPolicy(HitPolicy.Collect);
            When(x => x.Count > 5).Then(x => x.A = "First").Build();
            When(x => x.Count > 0).Then(x => x.B = "Second").Build();
        }
    }

    [Fact]
    public void HitPolicy_Collect_ShouldApplyAllMatches()
    {
        var output = EasyDecision.Evaluate<CollectPolicyDecision>(new MyInput { Count = 10 });
        Assert.Equal("First", output.A);
        Assert.Equal("Second", output.B);
    }
}
