namespace Flattery.Extensions;

internal static class SpanExtensions
{
    /// <summary>
    /// Filters the span by the provided predicate. Order is preserved. Note that this destroys the provided <paramref name="source" /> span.
    /// </summary>
    /// <typeparam name="T">The type of entry in the span.</typeparam>
    /// <param name="source">The span containing the source set of elements.</param>
    /// <param name="searchPredicate">The predicate on which to filter elements.</param>
    /// <returns><paramref name="source" /> with all elements for which <paramref name="searchPredicate" /> returned false removed.</returns>
    public static Span<T> Where<T>(this Span<T> source, Predicate<T> searchPredicate)
    {
        int writePointer = 0;

        // Walk through the span, moving the element at the READ pointer index to the WRITE pointer index. 
        // Results in a span<T> where the first N elements match the predicate condition and the trailing elements after N
        // are ignored as no longer part of the span. 
        for (int readPointer = 0; readPointer < source.Length; readPointer++)
        {
            if (searchPredicate(source[readPointer]))
            {
                if (readPointer != writePointer)
                {
                    source[writePointer] = source[readPointer];
                }

                writePointer += 1;
            }
        }

        // Return subset of the span that contains the correct N elements.
        return source[..writePointer];
    }

    /// <summary>
    /// Filters the span by the provided predicate. Order is preserved.
    /// </summary>
    /// <typeparam name="T">The type of entry in the span.</typeparam>
    /// <param name="source">The span containing the source set of elements.</param>
    /// <param name="predicate">The predicate on which to filter elements.</param>
    /// <returns>A new <see cref="Span{T}" />, with only the elements for which <paramref name="predicate" /> returned true.</returns>
    public static Span<T> Where<T>(this ReadOnlySpan<T> source, Predicate<T> predicate)
    {
        Span<T> mutableSpan = new T[source.Length];
        source.CopyTo(mutableSpan);

        return mutableSpan.Where(predicate);
    }
}
