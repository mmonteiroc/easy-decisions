using Xunit;
using EasyDecisions.Tests.TestModels;

namespace EasyDecisions.Tests;

public class AssignmentApiTests
{
    [Fact]
    public void Decision_WithNewAssignmentApi_ShouldWorkCorrectly()
    {
        var d = new Decision<MyInput, MyOutput>("AssignmentApiTest");

        // Using the new API
        d.When(x => x.Count > 10)
         .And(x => x.IsValid)
         .Then()
         .Set(x => x.A = "Greater than 10")
         .Set(x => x.TotalScore = 100)
         .Build();

        d.When(x => x.Count <= 10)
         .Then()
         .Set(x => x.A = "Less or equal 10")
         .Build();

        var input1 = new MyInput { Count = 15, IsValid = true };
        var output1 = d.Evaluate(input1);
        Assert.Equal("Greater than 10", output1.A);
        Assert.Equal(100, output1.TotalScore);

        var input2 = new MyInput { Count = 5 };
        var output2 = d.Evaluate(input2);
        Assert.Equal("Less or equal 10", output2.A);
    }

    [Fact]
    public void Decision_MixedApi_ShouldWorkCorrectly()
    {
        var d = new Decision<MyInput, MyOutput>("MixedApiTest");

        d.When(x => x.IsValid)
         .Then(x => x.A = "Valid")
         .Set(x => x.TotalScore = 50)
         .And(x => x.B = "Mixed")
         .Build();

        var output = d.Evaluate(new MyInput { IsValid = true });
        Assert.Equal("Valid", output.A);
        Assert.Equal(50, output.TotalScore);
        Assert.Equal("Mixed", output.B);
    }
}
