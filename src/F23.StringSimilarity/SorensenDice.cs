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
using F23.StringSimilarity.Interfaces;

// ReSharper disable LoopCanBeConvertedToQuery

namespace F23.StringSimilarity
{
    /// Similar to Jaccard index, but this time the similarity is computed as 2 * |V1
    /// inter V2| / (|V1| + |V2|). Distance is computed as 1 - cosine similarity.
    public class SorensenDice : ShingleBased, INormalizedStringDistance, INormalizedStringSimilarity
    {
        /// <summary>
        /// Sorensen-Dice coefficient, aka Sørensen index, Dice's coefficient or
        /// Czekanowski's binary (non-quantitative) index.
        ///
        /// The strings are first converted to boolean sets of k-shingles (sequences
        /// of k characters), then the similarity is computed as 2 * |A inter B| /
        /// (|A| + |B|). Attention: Sorensen-Dice distance (and similarity) does not
        /// satisfy triangle inequality.
        /// </summary>
        /// <param name="k"></param>
        public SorensenDice(int k) : base(k) { }

        /// <summary>
        /// Sorensen-Dice coefficient, aka Sørensen index, Dice's coefficient or
        /// Czekanowski's binary (non-quantitative) index.
        ///
        /// The strings are first converted to boolean sets of k-shingles (sequences
        /// of k characters), then the similarity is computed as 2 * |A inter B| /
        /// (|A| + |B|). Attention: Sorensen-Dice distance (and similarity) does not
        /// satisfy triangle inequality.
        /// Default k is 3.
        /// </summary>
        public SorensenDice() { }

        /// <summary>
        /// Similarity is computed as 2 * |A inter B| / (|A| + |B|).
        /// </summary>
        /// <param name="s1">The first string to compare.</param>
        /// <param name="s2">The second string to compare.</param>
        /// <returns>The computed Sorensen-Dice similarity.</returns>
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

            var union = new HashSet<string>();
            union.UnionWith(profile1.Keys);
            union.UnionWith(profile2.Keys);
            
            int inter = 0;

            foreach (var key in union)
            {
                if (profile1.ContainsKey(key) && profile2.ContainsKey(key))
                    inter++;
            }

            return 2.0 * inter / (profile1.Count + profile2.Count);
        }

        /// <summary>
        /// Returns 1 - similarity.
        /// </summary>
        /// <param name="s1">The first string to compare.</param>
        /// <param name="s2">The second string to compare.</param>
        /// <returns>1.0 - the computed similarity</returns>
        /// <exception cref="ArgumentNullException">If s1 or s2 is null.</exception>
        public double Distance(string s1, string s2)
            => 1 - Similarity(s1, s2);
    }
}
