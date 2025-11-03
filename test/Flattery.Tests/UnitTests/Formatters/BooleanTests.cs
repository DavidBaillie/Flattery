using Flattery.Formatters;

namespace Flattery.Tests.UnitTests.Formatters;

[TestFixture]
internal class BooleanTests
{
    [TestCase(true, "T")]
    [TestCase(false, "F")]
    public void FormatBooleanCreatesCorrectOutput(bool input, string output)
    {
        var result = ValueFormatter.FormatBooleanSpan(input, 1u, "T", "F");
        Assert.That(output.Equals(result.ToString(), StringComparison.InvariantCulture));
    }

    [TestCase(true, 2u, "Tr", "Fa", "Tr")]
    [TestCase(false, 2u, "Tr", "Fa", "Fa")]
    [TestCase(true, 5u, "Tr", "Fa", "Tr   ")]
    [TestCase(false, 5u, "Tr", "Fa", "Fa   ")]
    [TestCase(true, 5u, "True", "False", "True ")]
    [TestCase(false, 5u, "True", "False", "False")]
    public void FormatBooleanCreatesCorrectCustomOutput(bool input, uint fixedLength, string trueValue, string falseValue, string output)
    {
        var result = ValueFormatter.FormatBooleanSpan(input, fixedLength, trueValue, falseValue);
        Assert.That(output.Equals(result.ToString(), StringComparison.InvariantCulture));
    }

    [TestCase(true, 1u, "true", "false")]
    [TestCase(true, 0u, "t", "f")]
    [TestCase(true, 4u, "true", "false")]
    public void FormatBooleanWithTooLongValuesThrowsArgumentOutOfRangeException(bool input, uint fixedLength, string trueValue, string falseValue)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ValueFormatter.FormatBooleanSpan(input, fixedLength, trueValue, falseValue));
    }

    [Test]
    public void FormatObjectThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => ValueFormatter.FormatBooleanSpan(new object(), 1u, "T", "F"));
    }

    [Test]
    public void FormatIntegerThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => ValueFormatter.FormatBooleanSpan(1, 1u, "T", "F"));
    }

    [Test]
    public void FormatStringThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => ValueFormatter.FormatBooleanSpan("T", 1u, "T", "F"));
    }

    [Test]
    public void FormatFloatingPointThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => ValueFormatter.FormatBooleanSpan(1f, 1u, "T", "F"));
    }
}
