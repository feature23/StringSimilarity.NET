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
    public class Cosine : ShingleBased, INormalizedStringSimilarity, INormalizedStringDistance
    {
        /// <summary>
        /// Implements Cosine Similarity between strings.The strings are first
        /// transformed in vectors of occurrences of k-shingles(sequences of k
        /// characters). In this n-dimensional space, the similarity between the two
        /// strings is the cosine of their respective vectors.
        /// </summary>
        /// <param name="k"></param>
        public Cosine(int k) : base(k) { }

        /// <summary>
        /// Implements Cosine Similarity between strings.The strings are first
        /// transformed in vectors of occurrences of k-shingles(sequences of k
        /// characters). In this n-dimensional space, the similarity between the two
        /// strings is the cosine of their respective vectors.
        /// 
        /// Default k is 3.
        /// </summary>
        public Cosine() { }

        public double Similarity(string s1, string s2)
        {
            if (s1.Length < k || s2.Length < k)
            {
                return 0;
            }

            var profile1 = GetProfile(s1);
            var profile2 = GetProfile(s2);

            return DotProduct(profile1, profile2) / (Norm(profile1) * Norm(profile2));
        }

        /**
         * Compute the norm L2 : sqrt(Sum_i( v_i²)).
         *
         * @param profile
         * @return L2 norm
         */
        private static double Norm(IDictionary<string, int> profile)
        {
            double agg = 0;

            foreach (var entry in profile)
            {
                agg += 1.0 * entry.Value * entry.Value;
            }

            return Math.Sqrt(agg);
        }

        private static double DotProduct(IDictionary<string, int> profile1,
            IDictionary<string, int> profile2)
        {
            // Loop over the smallest map
            var small_profile = profile2;
            var large_profile = profile1;

            if (profile1.Count < profile2.Count)
            {
                small_profile = profile1;
                large_profile = profile2;
            }

            double agg = 0;
            foreach (var entry in small_profile)
            {
                if (!large_profile.ContainsKey(entry.Key)) continue;

                agg += 1.0 * entry.Value * large_profile[entry.Key];
            }

            return agg;
        }

        public double Distance(string s1, string s2)
            => 1.0 - Similarity(s1, s2);

        public double Similarity(IDictionary<string, int> profile1, IDictionary<string, int> profile2)
            => DotProduct(profile1, profile2)
            / (Norm(profile1) * Norm(profile2));
    }
}
