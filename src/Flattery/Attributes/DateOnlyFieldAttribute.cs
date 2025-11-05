using Flattery.Formatters;

namespace Flattery.Attributes;

internal sealed class DateOnlyFieldAttribute(uint start, uint end)
    : FlatFieldAttribute(start, end)
{
    /// <summary>
    /// Format string to use for the Date. Defaults to "yyyyMMdd".
    /// </summary>
    public string Format { get; init; } = "yyyyMMdd";

    public override ReadOnlySpan<char> FormatField(object? value)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(Convert.ToUInt32(Format.Length), FieldLength, nameof(Format));
        return ValueFormatter.FormatDateOnlySpan(value, Format);
    }
}
