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
using StringSimilarity.Interfaces;

namespace StringSimilarity
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
        /// 
        /// </summary>
        public Cosine() { }
        
        public double Similarity(string s1, string s2)
        {
            if (s1.Length < k || s2.Length < k)
            {
                return 0;
            }

            KShingling ks = new KShingling(k);
            int[] profile1 = ks.GetArrayProfile(s1);
            int[] profile2 = ks.GetArrayProfile(s2);

            return DotProduct(profile1, profile2) / (Norm(profile1) * Norm(profile2));
        }

        /**
         * Compute the norm L2 : sqrt(Sum_i( v_i²)).
         *
         * @param profile
         * @return L2 norm
         */
        protected static double Norm(int[] profile)
        {
            double agg = 0;

            foreach (var v in profile)
            {
                agg += (double)v * v;
            }

            return Math.Sqrt(agg);
        }

        protected static double DotProduct(int[] profile1, int[] profile2)
        {
            int length = Math.Min(profile1.Length, profile2.Length);

            double agg = 0;
            for (int i = 0; i < length; i++)
            {
                agg += profile1[i] * profile2[i];
            }
            return agg;
        }

        public double Distance(string s1, string s2)
            => 1.0 - Similarity(s1, s2);
    }
}
