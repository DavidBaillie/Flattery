using Flattery.Formatters;

namespace Flattery.Tests.UnitTests.Formatters;

[TestFixture]
internal class FloatingPointNumberTests
{
    [TestCase(1f, 1u, 3u, "1.0")]
    [TestCase(10f, 1u, 4u, "10.0")]
    [TestCase(100.23f, 1u, 5u, "100.2")]
    [TestCase(110.2345f, 2u, 6u, "110.23")]
    [TestCase(-110.2345f, 2u, 7u, "-110.23")]
    [TestCase(110.2345f, 2u, 10u, "0000110.23")]
    [TestCase(-110.2345f, 2u, 10u, "-000110.23")]
    public void FormatFloatingPointCreatesCorrectOutput(float input, uint decimalPlaces, uint fixedSize, string output)
    {
        var result = NumericFormatter.FormatFloatingPointSpan<float>(input, fixedSize, decimalPlaces);
        Assert.That(output.Equals(result.ToString(), StringComparison.InvariantCulture),
            message: $"Expected Floating Point output to be [{output}] but received [{result.ToString()}]");
    }

    [TestCase(100f, 0u, 0u)]
    [TestCase(100f, 1u, 1u)]
    [TestCase(100f, 3u, 1u)]
    [TestCase(100_000f, 4u, 1u)]
    [TestCase(100_000f, 5u, 1u)]
    public void FormatFloatingPointWithInvalidAvailableLengthThrowsArgumentOutOfRangeException(float input, uint fixedSize, uint decimalPlaces)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => NumericFormatter.FormatFloatingPointSpan<float>(input, fixedSize, decimalPlaces));
    }

    [Test]
    public void FormatObjectAsFloatingPointThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => NumericFormatter.FormatFloatingPointSpan<float>(new object(), 10, 1));
    }

    [Test]
    public void FormatStringAsFloatingPointThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => NumericFormatter.FormatFloatingPointSpan<float>("Something!", 10, 1));
    }

    [Test]
    public void FormatDoubleFloatingPointFloatingPointThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => NumericFormatter.FormatFloatingPointSpan<float>(123.45d, 10, 1));
    }

    [Test]
    public void FormatDecimalAsFloatingPointThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => NumericFormatter.FormatFloatingPointSpan<float>(123.45m, 10, 1));
    }
}
