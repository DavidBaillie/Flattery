using Flattery.Formatters;
using System.Numerics;

namespace Flattery.Attributes;

public sealed class SignedIntegerFieldAttribute<T>(uint start, uint end)
    : FlatFieldAttribute(start, end)
    where T : IBinaryInteger<T>, ISignedNumber<T>
{
    public override ReadOnlySpan<char> FormatField(object? value)
        => NumericFormatter.FormatIntegerSpan<T>(value, FieldLength);
}