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
    public class MetricLCS : IMetricStringDistance, INormalizedStringDistance
    {
        private readonly LongestCommonSubsequence lcs = new LongestCommonSubsequence();

        /// <summary>
        /// Distance metric based on Longest Common Subsequence, computed as
        /// 1 - |LCS(s1, s2)| / max(|s1|, |s2|).
        /// </summary>
        /// <param name="s1">The first string</param>
        /// <param name="s2">The second string</param>
        /// <returns>LCS distance metric</returns>
        public double Distance(string s1, string s2)
        {
            return 1.0
                    - (1.0 * lcs.Length(s1, s2))
                    / Math.Max(s1.Length, s2.Length);

        }
    }
}
