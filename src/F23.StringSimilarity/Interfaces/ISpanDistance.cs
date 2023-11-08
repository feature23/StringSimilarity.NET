using System;

namespace F23.StringSimilarity.Interfaces
{
    public interface ISpanDistance
    {
        /// <summary>
        /// Compute and return a measure of distance.
        /// Must be >= 0.
        ///
        /// This method operates on spans such as byte arrays.
        /// Note that, when used on bytes, string encodings that
        /// use more than one byte per codepoint (such as UTF-8)
        /// are not supported and will most likely return
        /// incorrect results.
        /// </summary>
        /// <param name="b1">The first span.</param>
        /// <param name="b2">The second span.</param>
        /// <returns>The measure of distance between the spans.</returns>
        double Distance<T>(ReadOnlySpan<T> b1, ReadOnlySpan<T> b2)
            where T : IEquatable<T>;
    }
}