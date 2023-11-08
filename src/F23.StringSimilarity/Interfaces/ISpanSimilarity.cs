using System;

namespace F23.StringSimilarity.Interfaces
{
    public interface ISpanSimilarity
    {
        /// <summary>
        /// Compute and return a measure of similarity between 2 spans.
        /// </summary>
        /// <param name="s1">The first span</param>
        /// <param name="s2">The second span</param>
        /// <returns>Similarity (0 means both spans are completely different)</returns>
        double Similarity<T>(ReadOnlySpan<T> s1, ReadOnlySpan<T> s2)
            where T : IEquatable<T>;
    }
}