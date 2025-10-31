using Flattery.Formatters;

namespace Flattery.Tests.UnitTests.Formatters;

[TestFixture]
internal class PositiveIntegerTests
{
    [TestCase(1u, 1u, "1")]
    [TestCase(100u, 3u, "100")]
    [TestCase(15u, 3u, "015")]
    [TestCase(250u, 5u, "00250")]
    [TestCase(250_000u, 6u, "250000")]
    [TestCase(250_000u, 10u, "0000250000")]
    public void FormatIntegerCreatesCorrectOutput(uint input, uint size, string output)
    {
        var result = NumericFormatter.FormatPositiveIntegerSpan<uint>(input, size);
        Assert.That(output.Equals(result.ToString(), StringComparison.InvariantCulture),
            message: $"Expected Integer output to be [{output}] but received [{result.ToString()}]");
    }

    [TestCase(100u, 0u)]
    [TestCase(100u, 1u)]
    [TestCase(100u, 2u)]
    [TestCase(100_000u, 4u)]
    [TestCase(100_000u, 5u)]
    public void FormatIntegerWithInvalidAvailableLengthThrowsArgumentOutOfRangeException(uint input, uint size)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => NumericFormatter.FormatPositiveIntegerSpan<uint>(input, size));
    }

    [TestCase(0u)]
    public void FormatIntegerWithInvalidLengthThrowsArgumentOutOfRangeException(uint fixedLength)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => NumericFormatter.FormatPositiveIntegerSpan<uint>(1, fixedLength));
    }

    [Test]
    public void FormatObjectAsIntegerThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => NumericFormatter.FormatPositiveIntegerSpan<uint>(new object(), 10));
    }

    [Test]
    public void FormatStringAsIntegerThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => NumericFormatter.FormatPositiveIntegerSpan<uint>("Something!", 10));
    }

    [Test]
    public void FormatFloatingPointIntegerThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => NumericFormatter.FormatPositiveIntegerSpan<uint>(123.45f, 10));
    }

    [Test]
    public void FormatDoubleFloatingPointIntegerThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => NumericFormatter.FormatPositiveIntegerSpan<uint>(123.45d, 10));
    }

    [Test]
    public void FormatDecimalAsIntegerThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => NumericFormatter.FormatPositiveIntegerSpan<uint>(123.45m, 10));
    }
}
