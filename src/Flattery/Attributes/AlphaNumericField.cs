using Flattery.Formatters;

namespace Flattery.Attributes;

/// <summary>
/// Using a <see cref="string"/>, generate an alphabetic and/or numeric output in the flat file row from the <paramref name="start"/> to the 
/// <paramref name="end"/> position. Takes the first valid N characters in the <see cref="string"/>, overflow is discarded.
/// </summary>
/// <param name="start">Start position to write to in the output</param>
/// <param name="end">End poisiton to write to in the output</param>
public sealed class AlphanumericFieldAttribute(uint start, uint end)
    : FlatFieldAttribute(start, end)
{
    /// <inheritdoc />
    public override ReadOnlySpan<char> FormatField(object? value)
        => AlphabeticFormatter.FormatAlphaNumericSpan(value, FieldLength);
}