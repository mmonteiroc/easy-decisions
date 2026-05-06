using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyDecisions.Extensions;

/// <summary>
/// Provides extension methods that mimic FEEL (Friendly Enough Expression Language) syntax 
/// to make decision rules more readable and expressive.
/// </summary>
public static class FeelExtensions
{
    /// <summary>
    /// Checks if the value is within the specified range (inclusive).
    /// Equivalent to [min..max] in FEEL.
    /// </summary>
    /// <example>
    /// <code>
    /// x => x.Age.IsBetween(18, 65)
    /// </code>
    /// </example>
    public static bool IsBetween<T>(this T value, T min, T max) where T : IComparable<T>
    {
        return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
    }

    /// <summary>
    /// Checks if the value is within the specified range with configurable inclusivity.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="includeMin">Whether to include the minimum value in the range.</param>
    /// <param name="includeMax">Whether to include the maximum value in the range.</param>
    public static bool IsBetween<T>(this T value, T min, T max, bool includeMin, bool includeMax) where T : IComparable<T>
    {
        var minCompare = value.CompareTo(min);
        var maxCompare = value.CompareTo(max);

        bool minOk = includeMin ? minCompare >= 0 : minCompare > 0;
        bool maxOk = includeMax ? maxCompare <= 0 : maxCompare < 0;

        return minOk && maxOk;
    }

    /// <summary>
    /// Checks if the value is strictly between min and max (exclusive).
    /// Equivalent to (min..max) in FEEL.
    /// </summary>
    public static bool IsBetweenExclusive<T>(this T value, T min, T max) where T : IComparable<T>
    {
        return value.CompareTo(min) > 0 && value.CompareTo(max) < 0;
    }

    /// <summary>
    /// Checks if the value is present in the specified collection.
    /// Equivalent to 'val in (a, b, c)' in FEEL.
    /// </summary>
    public static bool IsIn<T>(this T value, params T[] items)
    {
        return items.Contains(value);
    }

    /// <summary>
    /// Checks if the value is present in the specified collection.
    /// </summary>
    public static bool IsIn<T>(this T value, IEnumerable<T> items)
    {
        return items.Contains(value);
    }

    /// <summary>
    /// Checks if the value is NOT present in the specified collection.
    /// </summary>
    public static bool IsNotIn<T>(this T value, params T[] items)
    {
        return !items.Contains(value);
    }

    /// <summary>
    /// Checks if the value is greater than or equal to min.
    /// Equivalent to >= min in FEEL.
    /// </summary>
    public static bool IsAtLeast<T>(this T value, T min) where T : IComparable<T>
    {
        return value.CompareTo(min) >= 0;
    }

    /// <summary>
    /// Checks if the value is less than or equal to max.
    /// Equivalent to <= max in FEEL.
    /// </summary>
    public static bool IsAtMost<T>(this T value, T max) where T : IComparable<T>
    {
        return value.CompareTo(max) <= 0;
    }

    /// <summary>
    /// Checks if the value is strictly greater than min.
    /// Equivalent to > min in FEEL.
    /// </summary>
    public static bool IsGreaterThan<T>(this T value, T min) where T : IComparable<T>
    {
        return value.CompareTo(min) > 0;
    }

    /// <summary>
    /// Checks if the value is strictly less than max.
    /// Equivalent to < max in FEEL.
    /// </summary>
    public static bool IsLessThan<T>(this T value, T max) where T : IComparable<T>
    {
        return value.CompareTo(max) < 0;
    }
}
