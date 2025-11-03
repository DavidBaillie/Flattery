namespace Flattery.Formatters;

internal static class ValueFormatter
{
    public static ReadOnlySpan<char> FormatBooleanSpan(object? value, uint fixedLength, string trueValue, string falseValue)
    {
        // NULL assumed to be false
        if (value is null)
            return falseValue;

        if (value is not bool booleanValue)
            throw new ArgumentException($"This attribute can only be applied to {nameof(Boolean)}, but was applied to {value.GetType().Name}");

        ArgumentOutOfRangeException.ThrowIfGreaterThan(Convert.ToUInt32(trueValue.Length), fixedLength, nameof(trueValue));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(Convert.ToUInt32(falseValue.Length), fixedLength, nameof(falseValue));

        return booleanValue ?
            trueValue.PadRight(Convert.ToInt32(fixedLength)) :
            falseValue.PadRight(Convert.ToInt32(fixedLength));
    }


    public static ReadOnlySpan<char> FormatDateOnlySpan(object? value, uint fixedLength, string outputFormat)
    {
        if (outputFormat.Length > fixedLength)
            throw new InvalidOperationException($"Format string {outputFormat} is too long");

        if (value is null) return null;
        if (value is not DateOnly dateOnlyValue)
            throw new InvalidOperationException($"This attribute can only be applied to {nameof(DateOnly)}, but was applied to {value.GetType().Name}");

        return dateOnlyValue.ToString(outputFormat);
    }
}
