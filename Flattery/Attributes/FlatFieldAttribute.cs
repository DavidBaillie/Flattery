namespace Flattery.Attributes;

/// <summary>
/// The base definition used across all value typed flat file attributes. This class defines the basic implementation and 
/// structure that others will follow.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public abstract class FlatFieldAttribute : Attribute
{
    /// <summary>
    /// Starting array position of the elements in the resulting line of characters.
    /// </summary>
    public int Start { get; private init; }

    /// <summary>
    /// Ending (inclusive) array position of the elements in the resulting line of characters
    /// </summary>
    public int End { get; private init; }

    /// <summary>
    /// Utility for retrieving the length of the underlying field.
    /// </summary>
    protected int FieldLength => End - Start + 1;

    /// <summary>
    /// Initializes a new instance of the attribute, ensuring that minimal constraints are upheld.
    /// </summary>
    /// <param name="start">zero-indexed start position of the value in the output record</param>
    /// <param name="end">zero-indexed inclusive end position of the value in the output record.</param>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="end" /> is less than <paramref name="start" /> or if either value is negative.</exception>
    protected FlatFieldAttribute(int start, int end)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(end, start);
        ArgumentOutOfRangeException.ThrowIfNegative(start, nameof(start));
        ArgumentOutOfRangeException.ThrowIfNegative(end, nameof(end));

        Start = start;
        End = end;
    }

    /// <summary>
    /// Formats the value stored in this property into a string-like value.
    /// </summary>
    /// <param name="value">The value being formatted.</param>
    /// <returns>A readonly span of characters representing <paramref name="value" /> formatted according to the attribute's rules.</returns>
    public abstract ReadOnlySpan<char> FormatField(object? value);
}
