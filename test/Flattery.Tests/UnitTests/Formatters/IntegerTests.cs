using Flattery.Formatters;

namespace Flattery.Tests.UnitTests.Formatters;

[TestFixture]
internal class IntegerTests
{
    [TestCase(100, 3u, "100")]
    [TestCase(15, 3u, "015")]
    [TestCase(250, 5u, "00250")]
    [TestCase(-72, 3u, "-72")]
    [TestCase(-72, 4u, "-072")]
    [TestCase(-91, 5u, "-0091")]
    public void FormatIntegerCreatesCorrectOutput(int input, uint size, string output)
    {
        var result = NumericFormatter.FormatIntegerSpan<int>(input, Convert.ToUInt32(size));
        Assert.That(output.Equals(result.ToString(), StringComparison.InvariantCulture),
            message: $"Expected Integer output to be [{output}] but received [{result.ToString()}]");
    }

    [TestCase(100, 0u)]
    [TestCase(100, 1u)]
    [TestCase(100, 2u)]
    [TestCase(100_000, 4u)]
    [TestCase(100_000, 5u)]
    public void FormatIntegerWithInvalidAvailableLengthThrowsArgumentOutOfRangeException(int input, uint size)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => NumericFormatter.FormatIntegerSpan<int>(input, size));
    }

    [TestCase(0u)]
    [TestCase(1u)]
    public void FormatIntegerWithInvalidLengthThrowsArgumentOutOfRangeException(uint fixedLength)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => NumericFormatter.FormatIntegerSpan<int>(1, fixedLength));
    }

    [Test]
    public void FormatObjectAsIntegerThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => NumericFormatter.FormatIntegerSpan<int>(new object(), 10));
    }

    [Test]
    public void FormatStringAsIntegerThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => NumericFormatter.FormatIntegerSpan<int>("Something!", 10));
    }

    [Test]
    public void FormatFloatingPointIntegerThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => NumericFormatter.FormatIntegerSpan<int>(123.45f, 10));
    }

    [Test]
    public void FormatDoubleFloatingPointIntegerThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => NumericFormatter.FormatIntegerSpan<int>(123.45d, 10));
    }

    [Test]
    public void FormatDecimalAsIntegerThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => NumericFormatter.FormatIntegerSpan<int>(123.45m, 10));
    }
}
