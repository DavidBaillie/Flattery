using System.Numerics;

namespace Flattery.Attributes;

internal sealed class FloatingPointFieldAttribute<T>(int start, int end, char fillCharacter = '0')
    : FlatFieldAttribute(start, end)
    where T : IFloatingPoint<T>
{

    private char FillCharacter { get; set; } = fillCharacter;

    /// <summary>
    /// The number of digits to include before the decimal point. If not explicitly set,
    /// defaults to the largest number of digits the field can hold.
    /// </summary>
    public int IntegerDigits { get; init; } = -1;

    /// <summary>
    /// The number of digits to include after the decimal point.
    /// </summary>
    public required int DecimalDigits { get; init; }

    /// <summary>
    /// Whether or not to include the decimal point. If false, the integer and decimal
    /// parts will be smooshed together with no delimeter. Defaults to <see cref="true" />.
    /// </summary>
    public bool IncludeDecimal { get; init; } = true;

    /// <inheritdoc />
    public override ReadOnlySpan<char> FormatField(object? value)
    {
        var integerDigits = (IntegerDigits > 0) ? IntegerDigits : (FieldLength - DecimalDigits - (IncludeDecimal ? 1 : 0));
        var neededSpace = integerDigits + DecimalDigits + (IncludeDecimal ? 1 : 0);

        // Why are you using this if you want one or both halves to be missing...?
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(integerDigits);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(DecimalDigits);

        // Make sure the field size has enough space... mostly for development reasons
        if (neededSpace > FieldLength)
            throw new InvalidOperationException("Cannot fit the requested number of digits and/or decimal into the designated number of characters.");

        if (value is null) return null;
        if (value is not T integralValue)
            throw new InvalidOperationException($"This attribute can only be applied to {typeof(T).Name}, but was applied to {value.GetType().Name}");

        // Still need to determine how to structure a negative into the fields optionally
        // Will revisit later.
        ArgumentOutOfRangeException.ThrowIfNegative(integralValue);

        // Always include 1 extra digit for overflow checking
        string formatStr = $"{new string('0', integerDigits + 1)}.{new string('0', DecimalDigits)}";
        string formattedValue = integralValue.ToString(formatStr, null);

        if (!IncludeDecimal)
        {
            // Drop decimal, an extra 0 was included before to account for this
            formattedValue = formattedValue.Replace(".", "");
        }

        // Overflow happens if and only if the first digit is 0, because of the format string above.
        // If it is zero, we should drop it as it was added for this check only.
        if (formattedValue[0] != '0')
            throw new ArgumentException($"{integralValue} is too large to be formatted.");
        else
            formattedValue = formattedValue[1..];

        return formattedValue.PadLeft(FieldLength, '0');
    }
}
