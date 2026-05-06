using Xunit;
using EasyDecisions.Tests.TestModels;

namespace EasyDecisions.Tests;

public class AssignmentApiTests
{
    private class NewAssignmentApiDecision : Decision<MyInput, MyOutput>
    {
        public NewAssignmentApiDecision()
        {
            When(x => x.Count > 10)
             .And(x => x.IsValid)
             .Then()
             .Set(x => x.A = "Greater than 10")
             .Set(x => x.TotalScore = 100)
             .Build();

            When(x => x.Count <= 10)
             .Then()
             .Set(x => x.A = "Less or equal 10")
             .Build();
        }
    }

    [Fact]
    public void Decision_WithNewAssignmentApi_ShouldWorkCorrectly()
    {
        var input1 = new MyInput { Count = 15, IsValid = true };
        var output1 = EasyDecision.Evaluate<NewAssignmentApiDecision>(input1);
        Assert.Equal("Greater than 10", output1.A);
        Assert.Equal(100, output1.TotalScore);

        var input2 = new MyInput { Count = 5 };
        var output2 = EasyDecision.Evaluate<NewAssignmentApiDecision>(input2);
        Assert.Equal("Less or equal 10", output2.A);
    }

    private class MixedApiDecision : Decision<MyInput, MyOutput>
    {
        public MixedApiDecision()
        {
            When(x => x.IsValid)
             .Then(x => x.A = "Valid")
             .Set(x => x.TotalScore = 50)
             .And(x => x.B = "Mixed")
             .Build();
        }
    }

    [Fact]
    public void Decision_MixedApi_ShouldWorkCorrectly()
    {
        var output = EasyDecision.Evaluate<MixedApiDecision>(new MyInput { IsValid = true });
        Assert.Equal("Valid", output.A);
        Assert.Equal(50, output.TotalScore);
        Assert.Equal("Mixed", output.B);
    }
}
