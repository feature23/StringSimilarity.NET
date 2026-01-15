/*
 * The MIT License
 *
 * Copyright 2016 feature[23]
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using F23.StringSimilarity.Interfaces;

namespace F23.StringSimilarity
{
    /// <summary>
    /// Distance metric based on Longest Common Subsequence, from the notes "An
    /// LCS-based string metric" by Daniel Bakkelund.
    /// </summary>
    public class MetricLCS : IMetricStringDistance, INormalizedStringDistance, IMetricSpanDistance
    {
        /// <summary>
        /// Distance metric based on Longest Common Subsequence, computed as
        /// 1 - |LCS(s1, s2)| / max(|s1|, |s2|).
        /// </summary>
        /// <param name="s1">The first string to compare.</param>
        /// <param name="s2">The second string to compare.</param>
        /// <returns>LCS distance metric</returns>
        /// <exception cref="ArgumentNullException">If s1 or s2 is null.</exception>
        public double Distance(string s1, string s2)
            => Distance(s1.AsSpan(), s2.AsSpan());

        /// <summary>
        /// Calculates the normalized distance between two sequences based on their longest common subsequence.
        /// </summary>
        /// <remarks>The distance is calculated as: <code> 1.0 - (Length of Longest Common Subsequence /
        /// Maximum Length of the Two Sequences) </code> This method is case-sensitive for sequences of strings or characters.</remarks>
        /// <typeparam name="T">The type of elements in the sequences. Must implement <see cref="IEquatable{T}"/>.</typeparam>
        /// <param name="s1">The first sequence to compare. Cannot be null.</param>
        /// <param name="s2">The second sequence to compare. Cannot be null.</param>
        /// <returns>A value between 0.0 and 1.0 representing the normalized distance between the two sequences: <list
        /// type="bullet"> <item><description>Returns 0.0 if the sequences are identical.</description></item>
        /// <item><description>Returns 1.0 if the sequences have no common elements.</description></item>
        /// <item><description>Returns a value between 0.0 and 1.0 for partial similarity.</description></item> </list></returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="s1"/> or <paramref name="s2"/> is <see langword="null"/>.</exception>
        public double Distance<T>(ReadOnlySpan<T> s1, ReadOnlySpan<T> s2)
            where T : IEquatable<T>
        {
            if (s1 == null)
            {
                throw new ArgumentNullException(nameof(s1));
            }

            if (s2 == null)
            {
                throw new ArgumentNullException(nameof(s2));
            }

            if (s1.SequenceEqual(s2))
            {
                return 0;
            }

            int m_len = Math.Max(s1.Length, s2.Length);

            if (m_len == 0) return 0.0;

            return 1.0
                   - (1.0 * LongestCommonSubsequence.Length(s1, s2))
                   / m_len;
        }
    }
}
