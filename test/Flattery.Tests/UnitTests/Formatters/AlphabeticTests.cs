using Flattery.Formatters;

namespace Flattery.Tests.UnitTests.Formatters;

[TestFixture]
internal class AlphabeticTests
{
    [TestCase("alpha", int.MaxValue, "alpha")]
    [TestCase("beta", int.MaxValue, "beta")]
    [TestCase("123123beta", int.MaxValue, "beta")]
    [TestCase("b3ta", int.MaxValue, "bta")]
    [TestCase("alpha", 3, "alp")]
    public void FormatAlphabeticCreatesCorrectOuput(string input, int size, string output)
    {
        var result = AlphabeticFormatter.FormatAlphabeticSpan(input, Convert.ToUInt32(size));
        Assert.That(output.Equals(result.ToString(), StringComparison.InvariantCulture));
    }

    [Test]
    public void FormatIntegerThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => AlphabeticFormatter.FormatAlphabeticSpan(500, 10));
    }

    [Test]
    public void FormatObjectThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => AlphabeticFormatter.FormatAlphabeticSpan(new object(), 10));
    }
}
