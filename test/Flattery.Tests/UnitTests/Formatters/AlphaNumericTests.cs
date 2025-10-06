using Flattery.Formatters;

namespace Flattery.Tests.UnitTests.Formatters;

[TestFixture]
internal class AlphaNumericTests
{
    [TestCase("alpha", int.MaxValue, "alpha")]
    [TestCase("beta   ", int.MaxValue, "beta   ")]
    [TestCase("a1pha", int.MaxValue, "a1pha")]
    [TestCase("231123", int.MaxValue, "231123")]
    [TestCase("1234AA", 5, "1234A")]
    public void FormatsAlphaNumericsCorrectly(string input, int length, string output)
    {
        var result = AlphabeticFormatter.FormatAlphaNumericSpan(input, Convert.ToUInt32(length));
        Assert.That(result.ToString().Equals(output, StringComparison.InvariantCulture));
    }

    [Test]
    public void FormatIntegerThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => AlphabeticFormatter.FormatAlphaNumericSpan(500, 10));
    }

    [Test]
    public void FormatObjectThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => AlphabeticFormatter.FormatAlphaNumericSpan(new object(), 10));
    }
}
