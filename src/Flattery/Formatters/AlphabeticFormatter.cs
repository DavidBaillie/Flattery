using Flattery.Extensions;

namespace Flattery.Formatters;

internal static class AlphabeticFormatter
{
    public static ReadOnlySpan<char> FormatAlphabeticSpan(object? value, uint fixedLength)
    {
        // Can't do anything with NULL
        if (value is null)
            return null;

        // Attribute only applies to strings by definition of being an alphabetic field
        if (value is not string stringValue)
            throw new InvalidOperationException($"This attribute can only be applied to {nameof(String)}, but was applied to {value.GetType().Name}");

        var result = stringValue
            .AsSpan()
            .Where(x => !char.IsAsciiDigit(x));

        // Field may contain more characters than the flat file allows, in this case take the first N valid characters
        return result[..Math.Min(Convert.ToInt32(fixedLength), result.Length)];
    }

    public static ReadOnlySpan<char> FormatAlphaNumericSpan(object? value, uint fixedLength)
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
        return result[..Math.Min(Convert.ToInt32(fixedLength), result.Length)];
    }
}
