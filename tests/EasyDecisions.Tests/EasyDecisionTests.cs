using System;
using Xunit;
using EasyDecisions.Exceptions;

namespace EasyDecisions.Tests;

public class EasyDecisionTests
{
    public class SimpleInput
    {
        public int Value { get; set; }
    }

    public class SimpleOutput
    {
        public string Result { get; set; } = string.Empty;
    }

    public class MyDiscountDecision : Decision<SimpleInput, SimpleOutput>
    {
        public MyDiscountDecision()
        {
            HitPolicy = EasyDecisions.HitPolicy.First;

            When(x => x.Value > 100)
                .Then(x => x.Result = "Big Discount")
                .Build();

            When(x => x.Value > 50)
                .Then(x => x.Result = "Small Discount")
                .Build();
        }
    }

    public class InvalidDecision { }

    [Fact]
    public void EasyDecision_GenericWrapper_Of_ShouldEvaluateSuccessfully()
    {
        var input = new SimpleInput { Value = 75 };
        var result = EasyDecision<MyDiscountDecision>.Of(input);

        Assert.Equal("Small Discount", result.Result);
    }

    [Fact]
    public void EasyDecision_GenericWrapper_Of_InvalidType_ShouldThrowException()
    {
        var input = new SimpleInput { Value = 75 };
        
        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            EasyDecision<InvalidDecision>.Of(input);
        });
        
        Assert.Contains("does not inherit from Decision<TInput, TOutput>", ex.Message);
    }

    [Fact]
    public void EasyDecision_Static_Of_ShouldEvaluateSuccessfully()
    {
        var input = new SimpleInput { Value = 150 };
        var result = EasyDecision.Of<MyDiscountDecision, SimpleInput, SimpleOutput>(input);

        Assert.Equal("Big Discount", result.Result);
    }

    [Fact]
    public void EasyDecision_Static_Create_ShouldInstantiateAndEvaluateSuccessfully()
    {
        var input = new SimpleInput { Value = 150 };
        var decision = EasyDecision.Create<MyDiscountDecision>();
        
        var result = decision.Evaluate(input);

        Assert.Equal("Big Discount", result.Result);
    }
}
