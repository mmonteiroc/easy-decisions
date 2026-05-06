using System;
using Xunit;
using EasyDecisions;
using EasyDecisions.Tests.TestModels;

namespace EasyDecisions.Tests;

public class DecisionFactoryTests
{
    [Decision("STATUS_COLOR")]
    public class StatusColorDecision : IDecisionFactory<MyInput, MyOutput>
    {
        public Decision<MyInput, MyOutput> Create()
        {
            var decision = new Decision<MyInput, MyOutput>("STATUS_COLOR");
            decision.When(x => x.Count == 0).Then(x => x.A = "Red").Build();
            decision.When(x => x.Count == 1).Then(x => x.A = "Green").Build();
            decision.When(x => x.Count == 2).Then(x => x.A = "Yellow").Build();
            decision.When(x => x.Count == 3).Then(x => x.A = "Blue").Build();
            decision.When(x => x.Count >= 4).Then(x => x.A = "Purple").Build();
            return decision;
        }
    }

    [Decision("ANOTHER_DECISION")]
    public class AnotherDecision : IDecisionFactory<MyInput, MyOutput>
    {
        public Decision<MyInput, MyOutput> Create()
        {
            var decision = new Decision<MyInput, MyOutput>("ANOTHER_DECISION");
            decision.When(x => x.Category == "A").Then(x => x.A = "ResultA").Build();
            return decision;
        }
    }

    [DecisionFabricator("OBSOLETE_DECISION")]
#pragma warning disable CS0618 // Type or member is obsolete
    public class ObsoleteDecision : IDecisionFabricator<MyInput, MyOutput>
#pragma warning restore CS0618 // Type or member is obsolete
    {
        public Decision<MyInput, MyOutput> Create()
        {
            return new Decision<MyInput, MyOutput>("OBSOLETE_DECISION");
        }
    }

    [Theory]
    [InlineData(0, "Red")]
    [InlineData(1, "Green")]
    [InlineData(2, "Yellow")]
    [InlineData(3, "Blue")]
    [InlineData(4, "Purple")]
    [InlineData(10, "Purple")]
    public void DecisionFactory_Create_ShouldReturnConfiguredDecision(int count, string expectedColor)
    {
        var d = DecisionFactory.Create<MyInput, MyOutput>("STATUS_COLOR");
        Assert.NotNull(d);
        Assert.Equal("STATUS_COLOR", d.Name);

        var output = d.Evaluate(new MyInput { Count = count });
        Assert.Equal(expectedColor, output.A);
    }

    [Fact]
    public void DecisionFactory_Create_InvalidName_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            DecisionFactory.Create<MyInput, MyOutput>("NON_EXISTENT");
        });
        Assert.Contains("No decision factory found", ex.Message);
    }

    [Fact]
    public void DecisionFactory_Create_ObsoleteNaming_ShouldStillWork()
    {
        var d = DecisionFactory.Create<MyInput, MyOutput>("OBSOLETE_DECISION");
        Assert.NotNull(d);
        Assert.Equal("OBSOLETE_DECISION", d.Name);
    }

    public class DifferentInput { }
    public class DifferentOutput { public DifferentOutput() { } }

    [Fact]
    public void DecisionFactory_Create_WrongTypes_ThrowsInvalidOperationException()
    {
        // Try creating STATUS_COLOR with wrong generic types
        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            DecisionFactory.Create<DifferentInput, DifferentOutput>("STATUS_COLOR");
        });
        Assert.Contains("does not implement", ex.Message);
    }
}
