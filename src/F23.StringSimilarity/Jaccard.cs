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
using F23.StringSimilarity.Support;
// ReSharper disable LoopCanBeConvertedToQuery

namespace F23.StringSimilarity
{
    public class Jaccard : ShingleBased, IMetricStringDistance, INormalizedStringDistance, INormalizedStringSimilarity
    {
        public Jaccard(int k) : base(k) { }

        public Jaccard() { }

        /// <summary>
        /// Compute jaccard index: |A inter B| / |A union B|.
        /// </summary>
        /// <param name="s1">First string</param>
        /// <param name="s2">Second string</param>
        /// <returns>Similarity</returns>
        public double Similarity(string s1, string s2)
        {
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

            return 1.0 * inter / union.Count;
        }


        /// <summary>
        /// Distance is computed as 1 - similarity.
        /// </summary>
        /// <param name="s1">First string</param>
        /// <param name="s2">Second string</param>
        /// <returns>Distance</returns>
        public double Distance(string s1, string s2)
            => 1.0 - Similarity(s1, s2);
    }
}
