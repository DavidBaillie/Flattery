namespace Flattery.Attributes;

/// <summary>
/// Using a <see cref="bool"/>, generate an output in the flat file row from the <paramref name="start"/> position to the <paramref name="end"/> position.
/// </summary>
/// <param name="start">Start position to write from</param>
/// <param name="end">End position to write to</param>
/// <param name="trueValue">The value that will be used to present a TRUE <see cref="bool"/></param>
/// <param name="falseValue">The value that will be used to present a FALSE <see cref="bool"/></param>
public sealed class BooleanFieldAttribute(int start, int end, string trueValue = "Y", string falseValue = "N")
    : FlatFieldAttribute(start, end)
{
    /// <summary>
    /// The representation to use when the value is <see cref="true" />.
    /// </summary>
    public string True { get; init; } = trueValue;

    /// <summary>
    /// The representation to use when the value is <see cref="false" />.
    /// </summary>
    public string False { get; init; } = falseValue;

    /// <inheritdoc />
    public override ReadOnlySpan<char> FormatField(object? value)
    {
        // NULL assumed to be false
        if (value is null)
            return False;

        if (value is not bool booleanValue)
            throw new InvalidOperationException($"This attribute can only be applied to {nameof(Boolean)}, but was applied to {value.GetType().Name}");

        if (True.Length > FieldLength)
            throw new InvalidOperationException($"Cannot fit True value '{True}' in the allocated field size of {FieldLength}");

        if (False.Length > FieldLength)
            throw new InvalidOperationException($"Cannot fit False value '{False}' in the allocated field size of {FieldLength}");

        return booleanValue ? True : False;
    }
}
