using Xunit;

namespace EasyDecisions.Tests;

public class DecisionTests
{
    public class MyInput
    {
        public bool IsValid { get; set; }
        public int Count { get; set; }
    }

    public class MyOutput
    {
        public string? A { get; set; }
        public string? B { get; set; }
    }

    [Fact]
    public void Decision_Evaluate_ShouldApplyMatchingRules()
    {
        // Arrange
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

        // Act
        var output = d.Evaluate(inputs);

        // Assert
        Assert.Equal("hello", output.A);
        Assert.Equal("bye", output.B);
    }

    [Fact]
    public void Decision_Evaluate_ShouldNotApplyNonMatchingRules()
    {
        // Arrange
        var d = new Decision<MyInput, MyOutput>("MyDecision");

        d.When(x => x.IsValid)
         .And(x => x.Count > 5)
         .Then(x => x.A = "hello")
         .And(x => x.B = "bye")
         .Build();

        var inputs = new MyInput { IsValid = false, Count = 10 };

        // Act
        var output = d.Evaluate(inputs);

        // Assert
        Assert.Null(output.A);
        Assert.Null(output.B);
    }

    [DecisionFabricator("STATUS_COLOR")]
    public class StatusColorDecision : IDecisionFabricator<MyInput, MyOutput>
    {
        public Decision<MyInput, MyOutput> Create()
        {
            var decision = new Decision<MyInput, MyOutput>("STATUS_COLOR");
            decision.When(x => x.Count == 0)
                    .Then(x => x.A = "Red")
                    .Build();
            decision.When(x => x.Count == 1)
                    .Then(x => x.A = "Green")
                    .Build();
            decision.When(x => x.Count == 2)
                    .Then(x => x.A = "Yellow")
                    .Build();
            decision.When(x => x.Count == 3)
                    .Then(x => x.A = "Blue")
                    .Build();
            decision.When(x => x.Count == 4)
                    .Then(x => x.A = "Purple")
                    .Build();
            return decision;
        }
    }

    [Theory]
    [InlineData(0, "Red")]
    [InlineData(1, "Green")]
    [InlineData(2, "Yellow")]
    [InlineData(3, "Blue")]
    [InlineData(4, "Purple")]
    public void DecisionFactory_Create_ShouldReturnConfiguredDecision(int count, string expectedColor)
    {
        // Arrange & Act
        var d = DecisionFactory.Create<MyInput, MyOutput>("STATUS_COLOR");

        // Assert
        Assert.NotNull(d);
        Assert.Equal("STATUS_COLOR", d.Name);

        var output = d.Evaluate(new MyInput { Count = count });
        Assert.Equal(expectedColor, output.A);
    }
}
