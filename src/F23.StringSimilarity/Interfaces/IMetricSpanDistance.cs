using System;

namespace F23.StringSimilarity.Interfaces
{
    /// <summary>
    /// Span distances that implement this interface are metrics, which means:
    ///  - d(x, y) ≥ 0     (non-negativity, or separation axiom)
    ///  - d(x, y) = 0   if and only if   x = y     (identity, or coincidence axiom)
    ///  - d(x, y) = d(y, x)     (symmetry)
    ///  - d(x, z) ≤ d(x, y) + d(y, z)     (triangle inequality).
    /// </summary>
    public interface IMetricSpanDistance : ISpanDistance
    {
        /// <summary>
        /// Compute and return the metric distance.
        /// </summary>
        /// <param name="b1">The first span.</param>
        /// <param name="b2">The second span.</param>
        /// <returns>The metric distance.</returns>
        new double Distance<T>(ReadOnlySpan<T> b1, ReadOnlySpan<T> b2)
            where T : IEquatable<T>;
    }
}