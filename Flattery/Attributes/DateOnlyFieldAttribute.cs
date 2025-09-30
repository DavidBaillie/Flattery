namespace Flattery.Attributes;

internal sealed class DateOnlyFieldAttribute(int start, int end)
    : FlatFieldAttribute(start, end)
{
    /// <summary>
    /// Format string to use for the Date. Defaults to "yyyyMMdd".
    /// </summary>
    public string Format { get; init; } = "yyyyMMdd";

    public override ReadOnlySpan<char> FormatField(object? value)
    {
        if (Format.Length > FieldLength)
            throw new InvalidOperationException($"Format string {Format} is too long");

        if (value is null) return null;
        if (value is not DateOnly dateOnlyValue)
            throw new InvalidOperationException($"This attribute can only be applied to {nameof(DateOnly)}, but was applied to {value.GetType().Name}");

        return dateOnlyValue.ToString(Format);
    }
}
