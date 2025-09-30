namespace Flattery.Attributes;

/// <summary>
/// Using a <see cref="string"/>, generate an alphabetic and/or numeric output in the flat file row from the <paramref name="start"/> to the 
/// <paramref name="end"/> position. Takes the first valid N characters in the <see cref="string"/>, overflow is discarded.
/// </summary>
/// <param name="start">Start position to write to in the output</param>
/// <param name="end">End poisiton to write to in the output</param>
public sealed class AlphanumericFieldAttribute(int start, int end)
    : FlatFieldAttribute(start, end)
{
    /// <inheritdoc />
    public override ReadOnlySpan<char> FormatField(object? value)
    {
        // Can't process NULL
        if (value is null)
            return null;

        // Attribute only applies to strings by definition of being an alphanumeric field
        if (value is not string stringValue)
            throw new InvalidOperationException($"This attribute can only be applied to {nameof(String)}, but was applied to {value.GetType().Name}");

        var result = stringValue
                    .AsSpan();

        // Field may contain more characters than the flat file allows, in this case take the first N valid characters
        return result[..Math.Min(FieldLength, result.Length)];
    }
}