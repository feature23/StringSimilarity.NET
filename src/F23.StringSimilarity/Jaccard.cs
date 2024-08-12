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
using System.Collections.Generic;
using System.Linq;
using F23.StringSimilarity.Interfaces;

// ReSharper disable LoopCanBeConvertedToQuery

namespace F23.StringSimilarity
{
    /// <summary>
    /// Each input string is converted into a set of n-grams, the Jaccard index is
    /// then computed as |V1 inter V2| / |V1 union V2|.
    /// Like Q-Gram distance, the input strings are first converted into sets of
    /// n-grams (sequences of n characters, also called k-shingles), but this time
    /// the cardinality of each n-gram is not taken into account.
    /// Distance is computed as 1 - cosine similarity.
    /// Jaccard index is a metric distance.
    /// </summary>
    public class Jaccard : ShingleBased, IMetricStringDistance, INormalizedStringDistance, INormalizedStringSimilarity
    {
        /// <summary>
        /// The strings are first transformed into sets of k-shingles (sequences of k
        /// characters), then Jaccard index is computed as |A inter B| / |A union B|.
        /// The default value of k is 3.
        /// </summary>
        /// <param name="k"></param>
        public Jaccard(int k) : base(k) { }

        /// <summary>
        /// The strings are first transformed into sets of k-shingles (sequences of k
        /// characters), then Jaccard index is computed as |A inter B| / |A union B|.
        /// The default value of k is 3.
        /// </summary>
        public Jaccard() { }

        /// <summary>
        /// Compute jaccard index: |A inter B| / |A union B|.
        /// </summary>
        /// <param name="s1">The first string to compare.</param>
        /// <param name="s2">The second string to compare.</param>
        /// <returns>The Jaccard index in the range [0, 1]</returns>
        /// <exception cref="ArgumentNullException">If s1 or s2 is null.</exception>
        public double Similarity(string s1, string s2)
        {
            if (s1 == null)
            {
                throw new ArgumentNullException(nameof(s1));
            }

            if (s2 == null)
            {
                throw new ArgumentNullException(nameof(s2));
            }

            if (s1.Equals(s2))
            {
                return 1;
            }

            var profile1 = GetProfile(s1);
            var profile2 = GetProfile(s2);

            // SSNET Specific: use LINQ for more optimal distinct count
            var unionCount = profile1.Keys.Concat(profile2.Keys).Distinct().Count();

            int inter = profile1.Keys.Count + profile2.Keys.Count
                        - unionCount;

            return 1.0 * inter / unionCount;
        }


        /// <summary>
        /// Distance is computed as 1 - similarity.
        /// </summary>
        /// <param name="s1">The first string to compare.</param>
        /// <param name="s2">The second string to compare.</param>
        /// <returns>1 - the Jaccard similarity.</returns>
        /// <exception cref="ArgumentNullException">If s1 or s2 is null.</exception>
        public double Distance(string s1, string s2)
            => 1.0 - Similarity(s1, s2);
    }
}
