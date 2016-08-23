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
using F23.StringSimilarity.Support;

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
            KShingling ks = new KShingling(k);
            int[] profile1 = ks.GetArrayProfile(s1);
            int[] profile2 = ks.GetArrayProfile(s2);

            int length = Math.Max(profile1.Length, profile2.Length);

            profile1 = profile1.WithPadding(length);
            profile2 = profile2.WithPadding(length);

            int inter = 0;
            int union = 0;

            for (int i = 0; i < length; i++)
            {
                if (profile1[i] > 0 || profile2[i] > 0)
                {
                    union++;

                    if (profile1[i] > 0 && profile2[i] > 0)
                    {
                        inter++;
                    }
                }
            }

            return 1.0 * inter / union;
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
