using Xunit;

namespace EasyDecisions.Tests;

public class EasyDecisionTests
{
    public class SimpleInput
    {
        public int Value { get; set; }
        public bool IsPremium { get; set; }
    }

    public class SimpleOutput
    {
        public string Result { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }

    public class MyDiscountDecision : Decision<SimpleInput, SimpleOutput>
    {
        public MyDiscountDecision()
        {
            HitPolicy = EasyDecisions.HitPolicy.First;

            When(x => x.Value > 200)
                .And(x => x.IsPremium)
                .Then()
                .Set(x => x.Result = "Super VIP Discount")
                .Set(x => x.Reason = "High value and premium member");

            When(x => x.Value > 100)
                .Then()
                .Set(x => x.Result = "Big Discount");

            When(x => x.Value > 50)
                .Then()
                .Set(x => x.Result = "Small Discount");
        }
    }



    [Fact]
    public void EasyDecision_Evaluate_ShouldEvaluateSuccessfully()
    {
        var input = new SimpleInput { Value = 150 };
        var result = EasyDecision.Evaluate<MyDiscountDecision>(input);

        Assert.Equal("Big Discount", result.Result);
    }

    [Fact]
    public void EasyDecision_Evaluate_ComplexRule_ShouldMatchAndAssignBoth()
    {
        var input = new SimpleInput { Value = 250, IsPremium = true };
        var result = EasyDecision.Evaluate<MyDiscountDecision>(input);

        Assert.Equal("Super VIP Discount", result.Result);
        Assert.Equal("High value and premium member", result.Reason);
    }
}
