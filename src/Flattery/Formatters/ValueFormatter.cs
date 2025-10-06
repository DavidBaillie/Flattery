namespace Flattery.Formatters;

internal static class ValueFormatter
{
    public static ReadOnlySpan<char> FormatBooleanSpan(object? value, uint fixedLength, string trueValue, string falseValue)
    {
        // NULL assumed to be false
        if (value is null)
            return falseValue;

        if (value is not bool booleanValue)
            throw new InvalidOperationException($"This attribute can only be applied to {nameof(Boolean)}, but was applied to {value.GetType().Name}");

        if (trueValue.Length > fixedLength)
            throw new InvalidOperationException($"Cannot fit trueValue value '{trueValue}' in the allocated field size of {fixedLength}");

        if (falseValue.Length > fixedLength)
            throw new InvalidOperationException($"Cannot fit falseValue value '{falseValue}' in the allocated field size of {fixedLength}");

        return booleanValue ? trueValue : falseValue;
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
