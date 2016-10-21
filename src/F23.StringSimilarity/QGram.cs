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

namespace F23.StringSimilarity
{
    /// Q-gram distance, as defined by Ukkonen in "Approximate string-matching with
    /// q-grams and maximal matches". The distance between two strings is defined as
    /// the L1 norm of the difference of their profiles (the number of occurences of
    /// each n-gram): SUM( |V1_i - V2_i| ). Q-gram distance is a lower bound on
    /// Levenshtein distance, but can be computed in O(m + n), where Levenshtein
    /// requires O(m.n).
    public class QGram : ShingleBased, IStringDistance
    {
        /// <summary>
        /// Q-gram similarity and distance. Defined by Ukkonen in "Approximate
        /// string-matching with q-grams and maximal matches",
        /// http://www.sciencedirect.com/science/article/pii/0304397592901434 The
        /// distance between two strings is defined as the L1 norm of the difference
        /// of their profiles (the number of occurences of each k-shingle). Q-gram
        /// distance is a lower bound on Levenshtein distance, but can be computed in
        /// O(|A| + |B|), where Levenshtein requires O(|A|.|B|)
        /// </summary>
        /// <param name="k"></param>
        public QGram(int k) : base(k) { }

        /// <summary>
        /// Q-gram similarity and distance. Defined by Ukkonen in "Approximate
        /// string-matching with q-grams and maximal matches",
        /// http://www.sciencedirect.com/science/article/pii/0304397592901434 The
        /// distance between two strings is defined as the L1 norm of the difference
        /// of their profiles (the number of occurence of each k-shingle). Q-gram
        /// distance is a lower bound on Levenshtein distance, but can be computed in
        /// O(|A| + |B|), where Levenshtein requires O(|A|.|B|)
        /// Default k is 3.
        /// </summary>
        public QGram() { }

        /// <summary>
        /// The distance between two strings is defined as the L1 norm of the
        /// difference of their profiles (the number of occurence of each k-shingle).
        /// </summary>
        /// <param name="s1">The first string</param>
        /// <param name="s2">The second string</param>
        /// <returns></returns>
        public double Distance(string s1, string s2)
        {
            var profile1 = GetProfile(s1);
            var profile2 = GetProfile(s2);

            var union = new HashSet<string>();
            union.UnionWith(profile1.Keys);
            union.UnionWith(profile2.Keys);

            int agg = 0;
            foreach (var key in union)
            {
                int v1 = 0;
                int v2 = 0;

                if (profile1.ContainsKey(key))
                {
                    v1 = profile1[key];
                }

                if (profile2.ContainsKey(key))
                {
                    v2 = profile2[key];
                }

                agg += Math.Abs(v1 - v2);
            }

            return agg;
        }
    }
}
