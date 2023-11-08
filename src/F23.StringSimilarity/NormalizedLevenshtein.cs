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
    /// This distance is computed as levenshtein distance divided by the length of
    /// the longest string. The resulting value is always in the interval [0.0 1.0]
    /// but it is not a metric anymore! The similarity is computed as 1 - normalized
    /// distance.
    public class NormalizedLevenshtein : INormalizedStringDistance, INormalizedStringSimilarity, INormalizedSpanDistance, INormalizedSpanSimilarity
    {
        private readonly Levenshtein l = new Levenshtein();

        /// <summary>
        /// Compute distance as Levenshtein(s1, s2) / max(|s1|, |s2|).
        /// </summary>
        /// <param name="s1">The first string to compare.</param>
        /// <param name="s2">The second string to compare.</param>
        /// <returns>The computed distance in the range [0, 1]</returns>
        /// <exception cref="ArgumentNullException">If s1 or s2 is null.</exception>
        public double Distance(string s1, string s2)
            => Distance(s1.AsSpan(), s2.AsSpan());
        
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
                return 0.0;
            }

            int m_len = Math.Max(s1.Length, s2.Length);

            if (m_len == 0)
            {
                return 0.0;
            }

            return l.Distance(s1, s2) / m_len;
        }

        /// <summary>
        /// Return 1 - distance.
        /// </summary>
        /// <param name="s1">The first string to compare.</param>
        /// <param name="s2">The second string to compare.</param>
        /// <returns>1 - distance</returns>
        /// <exception cref="ArgumentNullException">If s1 or s2 is null.</exception>
        public double Similarity(string s1, string s2)
            => 1.0 - Distance(s1, s2);
        
        public double Similarity<T>(ReadOnlySpan<T> s1, ReadOnlySpan<T> s2)
            where T : IEquatable<T>
            => 1.0 - Distance(s1, s2);
    }
}
