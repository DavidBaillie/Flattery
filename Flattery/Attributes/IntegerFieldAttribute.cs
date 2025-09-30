using System.Numerics;

namespace Flattery.Attributes;


public sealed class IntegerFieldAttribute<T>(int start, int end)
    : FlatFieldAttribute(start, end)
    where T : IBinaryInteger<T>
{
    /// <summary>
    /// The number of digits to allow in the field. If not set, defaults to the
    /// largest number of digits that will fit in the field.
    /// </summary>
    public int? Digits { get; init; }

    public override ReadOnlySpan<char> FormatField(object? value)
    {
        var digits = Digits ?? FieldLength;

        if (value is null)
            return null;

        if (value is not T integralValue)
            throw new InvalidOperationException($"This attribute can only be applied to {typeof(T).Name}, but was applied to {value.GetType().Name}");

        // Still need to determine how to structure a negative into the fields optionally
        // Will revisit later.
        ArgumentOutOfRangeException.ThrowIfNegative(integralValue);

        // Make sure the field size has enough space... mostly for development reasons
        if (digits > FieldLength)
            throw new InvalidOperationException("Cannot fit the requested number of digits into the designated number of characters.");

        string formattedValue = integralValue.ToString(new string('0', digits + 1), null);

        // Overflow happens if and only if the first digit is 0, because of the format string above.
        // If it is zero, we should drop it as it was added for this check only.
        if (formattedValue[0] != '0')
            throw new ArgumentException($"{integralValue} is too large to be formatted.");
        else
            formattedValue = formattedValue[1..];

        return formattedValue.PadLeft(FieldLength, '0');
    }
}