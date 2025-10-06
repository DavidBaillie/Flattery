namespace Flattery;

/// <summary>
/// Builder capable of generating flat files
/// </summary>
public interface IFlatFileBuilder
{
    /// <summary>
    /// Yeilds the largest end position found inside the record attributes.
    /// </summary>
    /// <param name="record">Record to read</param>
    /// <returns>Largest end position in the record</returns>
    int GetRecordLength(object record);

    /// <summary>
    /// Appends a new record to the builder that will be included in the output. Row length will be decided by the 
    /// largest end position in the record.
    /// </summary>
    /// <param name="record">Record/Model containing the content to build</param>
    /// <returns>Cascading reference for chain calls</returns>
    /// <exception cref="InvalidOperationException">Thrown when the record itself in invalid for content generation</exception>
    FlatFileBuilder AppendRecord(object record);

    /// <summary>
    /// Appends a new record to the builder that will be included in the output.
    /// </summary>
    /// <param name="record">Record/Model containing the content to build</param>
    /// <param name="fixedRowLength">Length of the row for this record</param>
    /// <returns>Cascading reference for chain calls</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="fixedRowLength"/> is too small to fit all desired content from the record</exception>
    /// <exception cref="InvalidOperationException">Thrown when the record itself in invalid for content generation</exception>
    FlatFileBuilder AppendRecord(object record, int fixedRowLength);

    /// <summary>
    /// Starts a new flat file by clearing the internal file data.
    /// </summary>
    /// <returns>The same instance, for chaining calls.</returns>
    string Build();

    /// <summary>
    /// Retrieves a copy of the current flat file in the builder.
    /// </summary>
    /// <returns>The current flat file in the builder.</returns>
    FlatFileBuilder Clear();
}