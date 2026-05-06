namespace EasyDecisions.Tests.TestModels;

public class TestDecision<TInput, TOutput> : Decision<TInput, TOutput> where TOutput : new()
{
    public TestDecision() : base() { }
    public TestDecision(string name) : base(name) { }
}
