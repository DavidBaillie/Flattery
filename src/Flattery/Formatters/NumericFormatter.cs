using System.Globalization;
using System.Numerics;

namespace Flattery.Formatters;

internal static class NumericFormatter
{
    /// <summary>
    /// Takes a value type that is unsigned and of some integer representation (no decimals) and formats it into the desired string of N length.
    /// </summary>
    /// <typeparam name="T">Type of non-signed integer to format</typeparam>
    /// <param name="value">Value to format</param>
    /// <param name="fixedLength">Fixed length of the resulting span</param>
    /// <returns><see cref="ReadOnlySpan{T}"/> of <paramref name="fixedLength"/> characters</returns>
    /// <exception cref="InvalidOperationException">Thrown when the provided input is not of the required type</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the input or output values are not within the defined ranges.</exception>
    public static ReadOnlySpan<char> FormatPositiveIntegerSpan<T>(object? value, uint fixedLength)
        where T : IBinaryInteger<T>, IUnsignedNumber<T>
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(fixedLength, nameof(fixedLength));

        if (value is null)
            return null;

        if (value is not T unsignedValue)
            throw new InvalidOperationException($"This attribute can only be applied to {typeof(T).Name}, but was applied to {value.GetType().Name}");

        string formattedValue = unsignedValue.ToString($"D{fixedLength}", CultureInfo.InvariantCulture);

        if (formattedValue.Length > fixedLength)
            throw new ArgumentOutOfRangeException($"Cannot format the value '{unsignedValue}' because it is larger than the allows space of {fixedLength} digits.");

        return formattedValue;
    }

    /// <summary>
    /// Takes a value type that is some signed integer representation and formats it into the desired string of N length
    /// </summary>
    /// <typeparam name="T">Type of signed integer value</typeparam>
    /// <param name="value">Value to format</param>
    /// <param name="fixedLength">Fixed length of the output</param>
    /// <returns><see cref="ReadOnlySpan{T}"/> of <paramref name="fixedLength"/> characters.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the provided input is not of the required type</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the input or output values are not within the defined ranges.</exception>
    public static ReadOnlySpan<char> FormatIntegerSpan<T>(object? value, uint fixedLength)
        where T : IBinaryInteger<T>, ISignedNumber<T>
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual<uint>(fixedLength, 1, nameof(fixedLength));

        if (value is null)
            return null;

        if (value is not T signedValue)
            throw new InvalidOperationException($"This attribute can only be applied to {typeof(T).Name}, but was applied to {value.GetType().Name}");

        string formattedValue = signedValue.ToString($"D{fixedLength - 1}", CultureInfo.InvariantCulture);

        if (formattedValue.Length > fixedLength)
            throw new ArgumentOutOfRangeException($"Cannot format the value '{signedValue}' because it is larger than the allows space of {fixedLength} digits.");

        return formattedValue;
    }

    /// <summary>
    /// Takes a floating point value and formats it into the desired string of N length
    /// </summary>
    /// <typeparam name="T">Type of floating point value to format</typeparam>
    /// <param name="value">Value to format</param>
    /// <param name="fixedLength">Fixed length of the output</param>
    /// <param name="integerDigits">Number of allowed digits for the integer piece</param>
    /// <param name="decimalDigits">Number of allowed digits for the decimal piece</param>
    /// <returns><see cref="ReadOnlySpan{T}"/> of <paramref name="fixedLength"/> characters</returns>
    /// <exception cref="InvalidOperationException">Thrown when the provided input is not of the required type</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the input or output values are not within the defined ranges.</exception>
    public static ReadOnlySpan<char> FormatFloatingPointSpan<T>(object? value, uint fixedLength, uint integerDigits, uint decimalDigits)
        where T : IFloatingPoint<T>
    {
        // Why are you using this if you want one or both halves to be missing...?
        ArgumentOutOfRangeException.ThrowIfZero(integerDigits, nameof(integerDigits));
        ArgumentOutOfRangeException.ThrowIfZero(decimalDigits, nameof(decimalDigits));

        // Integer digits + decimal digits + decimal point + negative sign (maybe)
        if (integerDigits + decimalDigits + 2 > fixedLength)
            throw new InvalidOperationException("Cannot fit the requested number of digits and/or decimal into the designated number of characters.");

        if (value is null)
            return null;

        if (value is not T floatingPointValue)
            throw new InvalidOperationException($"This attribute can only be applied to {typeof(T).Name}, but was applied to {value.GetType().Name}");

        var result = floatingPointValue.ToString($"F{decimalDigits}", CultureInfo.InvariantCulture);

        if (result.Split('.')[0].Length > integerDigits)
            throw new ArgumentOutOfRangeException($"Cannot format the value '{floatingPointValue}' because it requires more integer digits than {integerDigits} for formatting.");

        // Jank formatting to deal with the negative sign and padding stuffs
        if (result.StartsWith('-'))
        {
            // If it's negative:
            // Remove the negative sign (first char) and then padd to N - 1 zeros
            // After padding, add the negative sign back to the very front
            result = "-" + result[1..].PadLeft(Convert.ToInt32(integerDigits - 1), '0');
        }
        else
        {
            result = result.PadLeft(Convert.ToInt32(integerDigits), '0');
        }

        return result;
    }
}
