using System;
using System.Collections.Generic;
using EasyDecisions.Extensions;
using Xunit;

namespace EasyDecisions.Tests;

public class FeelExtensionsTests
{
    [Theory]
    [InlineData(20, 18, 65, true)]
    [InlineData(18, 18, 65, true)]
    [InlineData(65, 18, 65, true)]
    [InlineData(17, 18, 65, false)]
    [InlineData(66, 18, 65, false)]
    public void IsBetween_Int_ReturnsExpected(int value, int min, int max, bool expected)
    {
        Assert.Equal(expected, value.IsBetween(min, max));
    }

    [Theory]
    [InlineData("b", "a", "c", true)]
    [InlineData("a", "a", "c", true)]
    [InlineData("c", "a", "c", true)]
    [InlineData("d", "a", "c", false)]
    public void IsBetween_String_ReturnsExpected(string value, string min, string max, bool expected)
    {
        Assert.Equal(expected, value.IsBetween(min, max));
    }

    [Theory]
    [InlineData(20, 18, 65, true, true, true)]
    [InlineData(18, 18, 65, true, true, true)]
    [InlineData(18, 18, 65, false, true, false)]
    [InlineData(65, 18, 65, true, true, true)]
    [InlineData(65, 18, 65, true, false, false)]
    public void IsBetween_Configurable_ReturnsExpected(int value, int min, int max, bool includeMin, bool includeMax, bool expected)
    {
        Assert.Equal(expected, value.IsBetween(min, max, includeMin, includeMax));
    }

    [Theory]
    [InlineData(20, 18, 65, true)]
    [InlineData(18, 18, 65, false)]
    [InlineData(65, 18, 65, false)]
    public void IsBetweenExclusive_ReturnsExpected(int value, int min, int max, bool expected)
    {
        Assert.Equal(expected, value.IsBetweenExclusive(min, max));
    }

    [Theory]
    [InlineData(1, new[] { 1, 2, 3 }, true)]
    [InlineData(4, new[] { 1, 2, 3 }, false)]
    public void IsIn_IEnumerable_ReturnsExpected(int value, int[] items, bool expected)
    {
        Assert.Equal(expected, value.IsIn((IEnumerable<int>)items));
    }

    [Fact]
    public void IsIn_Params_ReturnsExpected()
    {
        Assert.True(1.IsIn(1, 2, 3));
        Assert.False(4.IsIn(1, 2, 3));
    }

    [Fact]
    public void IsNotIn_ReturnsExpected()
    {
        Assert.False(1.IsNotIn(1, 2, 3));
        Assert.True(4.IsNotIn(1, 2, 3));
    }

    [Theory]
    [InlineData(18, 18, true)]
    [InlineData(19, 18, true)]
    [InlineData(17, 18, false)]
    public void IsAtLeast_ReturnsExpected(int value, int min, bool expected)
    {
        Assert.Equal(expected, value.IsAtLeast(min));
    }

    [Theory]
    [InlineData(65, 65, true)]
    [InlineData(64, 65, true)]
    [InlineData(66, 65, false)]
    public void IsAtMost_ReturnsExpected(int value, int max, bool expected)
    {
        Assert.Equal(expected, value.IsAtMost(max));
    }

    [Fact]
    public void IsBetween_DateTime_ReturnsExpected()
    {
        var now = DateTime.Now;
        var past = now.AddDays(-1);
        var future = now.AddDays(1);

        Assert.True(now.IsBetween(past, future));
        Assert.False(past.AddDays(-1).IsBetween(past, future));
    }
}
