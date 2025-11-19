using Flattery.Attributes;
using System.Reflection;

namespace Flattery;

/// <summary>
/// Builder takes an record/model, reads all FlatField attributes found on it and builds a single row for a flat file based on the contents.
/// Once all records have been added, allows for building the contents into a single stream to be saved into some external system.
/// </summary>
public class FlatFileBuilder : IFlatFileBuilder
{
    private readonly List<string> allRecords = [];

    /// <inheritdoc />
    public virtual int GetRecordLength(object record)
    {
        return record
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(prop => (Property: prop, FieldAttribute: prop.GetCustomAttribute<FlatFieldAttribute>()))
            .Where(x => x.FieldAttribute is not null)
            .Max(x => Convert.ToInt32(x.FieldAttribute!.End));
    }

    /// <inheritdoc />
    public virtual FlatFileBuilder AppendRecord(object record) => AppendRecord(record, GetRecordLength(record));

    /// <inheritdoc />
    public virtual FlatFileBuilder AppendRecord(object record, int fixedRowLength)
    {
        // Can't process null args
        ArgumentNullException.ThrowIfNull(record);

        // Check that the attributes match the expected length provided
        var attributeRequiredLength = GetRecordLength(record);
        if (fixedRowLength < attributeRequiredLength)
            throw new ArgumentOutOfRangeException($"Cannot build flat file for record because the provided fixed length of {fixedRowLength} " +
                $"is less than the required length of {attributeRequiredLength} defined by the attributes on the record.");

        // Grab all the FlatFile properties on the model, ordered by start position such that
        // we can walk through the properties and trust that property N + 1 should be immediately after property N
        var fieldsInRecord = record
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(prop => (Property: prop, FieldAttribute: prop.GetCustomAttribute<FlatFieldAttribute>()))
            .Where(x => x.FieldAttribute is not null)
            .OrderBy(x => x.FieldAttribute!.Start)
            .Select(x => (x.Property, x.FieldAttribute!));

        if (!fieldsInRecord.Any())
            throw new ArithmeticException("Provided record does not contain any Flat File Attributes and will result in no content being generated inside the builder.");

        var previousPropertyName = string.Empty;
        var previousEndPosition = -1;

        // Allocate a span on the stack so that we have a fixed piece of memory to work with 
        // while parsing the properties into a fixed string structure. Once work is done on this 
        // piece of memory we can allocate a space on the heap for storage based on the users needs.
        Span<char> stringSpan = stackalloc char[fixedRowLength + 1];
        stringSpan.Fill(' ');

        foreach (var (property, fieldAttribute) in fieldsInRecord)
        {
            // If the fields overlap, yell at the dev for causing damage to the output
            if (fieldAttribute.Start <= previousEndPosition)
                throw new InvalidOperationException(
                    $"Writing {property.Name} to the record would overwrite data that was already written by {previousPropertyName}. " +
                    $"This likely means the {nameof(FlatFieldAttribute.Start)} or {nameof(FlatFieldAttribute.End)} of one or both has an overlapping value with another " +
                    $"attribute found on the record.");

            // Copy data to correct index
            fieldAttribute
                .FormatField(property.GetValue(record))
                .CopyTo(stringSpan[Convert.ToInt32(fieldAttribute.Start)..Convert.ToInt32(fieldAttribute.End + 1)]);

            previousPropertyName = property.Name;
            previousEndPosition = Convert.ToInt32(fieldAttribute.End);
        }

        // Add the resulting span to the string builder for when the dev wants an output
        allRecords.Add(stringSpan.ToString());
        return this;
    }

    /// <inheritdoc />
    public virtual FlatFileBuilder Clear()
    {
        allRecords.Clear();
        return this;
    }

    /// <inheritdoc />
    public virtual string Build()
    {
        return string.Join("\n", allRecords);
    }
}
