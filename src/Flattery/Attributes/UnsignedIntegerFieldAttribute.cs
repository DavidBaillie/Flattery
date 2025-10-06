using Flattery.Formatters;
using System.Numerics;

namespace Flattery.Attributes;

public sealed class UnsignedIntegerFieldAttribute<T>(uint start, uint end)
    : FlatFieldAttribute(start, end)
    where T : IBinaryInteger<T>, IUnsignedNumber<T>
{
    public override ReadOnlySpan<char> FormatField(object? value)
        => NumericFormatter.FormatPositiveIntegerSpan<T>(value, FieldLength);
}