using Flattery.Attributes;

namespace Flattery.Tests.UnitTests.Outputs;

[TestFixture]
internal sealed class FileGenerationTests
{
    [Test]
    public void WhenCreateSingleLineStringOutputIsCorrect()
    {
        var expected = "John       0180002.13Y";
        var source = new SimpleUserExample()
        {
            Name = "John",
            Age = 18,
            BankBalance = 2.13,
            AccountEnabled = true
        };

        var builder = new FlatFileBuilder();
        builder.AppendRecord(source);

        var output = builder.Build();

        Assert.That(output, Is.EqualTo(expected),
            message: $"Flat file builder did not generated the expected output.\nActual: [{output}]\nExpected: [{expected}]");
    }

    [Test]
    public void WhenCreateMultiLineStringOutputIsCorrect()
    {
        var expected = "John       0180002.13Y\nJohn       0180002.13Y";
        var source = new SimpleUserExample()
        {
            Name = "John",
            Age = 18,
            BankBalance = 2.13,
            AccountEnabled = true
        };

        var builder = new FlatFileBuilder();
        builder.AppendRecord(source);
        builder.AppendRecord(source);

        var output = builder.Build();

        Assert.That(output, Is.EqualTo(expected),
            message: $"Flat file builder did not generated the expected output.\nActual: [{output}]\nExpected: [{expected}]");
    }
}

file class SimpleUserExample
{
    [AlphabeticField(0, 10)]
    public required string Name { get; set; }

    [UnsignedIntegerField<uint>(11, 13)]
    public required uint Age { get; set; }

    [FloatingPointField<double>(14, 20, DecimalDigits = 2)]
    public required double BankBalance { get; set; }

    [BooleanField(21, 21)]
    public required bool AccountEnabled { get; set; }
}
