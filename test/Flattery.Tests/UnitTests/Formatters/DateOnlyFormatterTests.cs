using Flattery.Formatters;
using System.Globalization;

namespace Flattery.Tests.UnitTests.Formatters;

[TestFixture]
internal class DateOnlyFormatterTests
{
    [TestCase("2025-01-01", "yyyyMMdd", "20250101")]
    [TestCase("2025-10-21", "yyyyMMdd", "20251021")]
    [TestCase("2025-10-21", "yyyyddMM", "20252110")]
    [TestCase("2020-12-01", "yyyyMMdd", "20201201")]
    public void FormatDateOnlyCreatesCorrectOutput(string input, string format, string output)
    {
        var result = ValueFormatter.FormatDateOnlySpan(DateOnly.ParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture), format);
        Assert.That(output.Equals(result.ToString(), StringComparison.InvariantCulture),
            $"Received {input} using format {format} and expected {output} but received {result}");
    }

    [Test]
    public void FormatObjectThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => ValueFormatter.FormatDateOnlySpan(new object(), "yyyyMMdd"));
    }

    [Test]
    public void FormatIntegerThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => ValueFormatter.FormatDateOnlySpan(1, "yyyyMMdd"));
    }

    [Test]
    public void FormatStringThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => ValueFormatter.FormatDateOnlySpan("T", "yyyyMMdd"));
    }

    [Test]
    public void FormatFloatingPointThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => ValueFormatter.FormatDateOnlySpan(1f, "yyyyMMdd"));
    }
}
