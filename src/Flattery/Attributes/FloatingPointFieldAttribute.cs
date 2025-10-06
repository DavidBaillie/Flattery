using Flattery.Formatters;
using System.Numerics;

namespace Flattery.Attributes;

internal sealed class FloatingPointFieldAttribute<T>(uint start, uint end)
    : FlatFieldAttribute(start, end)
    where T : IFloatingPoint<T>
{
    /// <summary>
    /// The number of digits to include before the decimal point.
    /// </summary>
    public required uint IntegerDigits { get; init; }

    /// <summary>
    /// The number of digits to include after the decimal point.
    /// </summary>
    public required uint DecimalDigits { get; init; }

    /// <inheritdoc />
    public override ReadOnlySpan<char> FormatField(object? value)
        => NumericFormatter.FormatFloatingPointSpan<T>(value, FieldLength, IntegerDigits, DecimalDigits);
}
