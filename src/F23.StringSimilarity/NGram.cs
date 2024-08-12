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
// ReSharper disable ConvertIfStatementToReturnStatement
// ReSharper disable SuggestVarOrType_Elsewhere
// ReSharper disable JoinDeclarationAndInitializer
// ReSharper disable TooWideLocalVariableScope

namespace F23.StringSimilarity
{
    /// <summary>
    /// N-Gram Similarity as defined by Kondrak, "N-Gram Similarity and Distance",
    /// String Processing and Information Retrieval, Lecture Notes in Computer
    /// Science Volume 3772, 2005, pp 115-126.
    /// 
    /// The algorithm uses affixing with special character '\n' to increase the
    /// weight of first characters. The normalization is achieved by dividing the
    /// total similarity score the original length of the longest word.
    /// 
    /// total similarity score the original length of the longest word.
    /// </summary>
    public class NGram : INormalizedStringDistance
    {
        private const int DEFAULT_N = 2;
        private readonly int n;

        public NGram() : this(DEFAULT_N) { }

        public NGram(int n)
        {
            this.n = n;
        }

        /// <summary>
        /// Compute n-gram distance.
        /// </summary>
        /// <param name="s0">The first string to compare.</param>
        /// <param name="s1">The second string to compare.</param>
        /// <returns>The computed n-gram distance in the range [0, 1]</returns>
        /// <exception cref="ArgumentNullException">If s0 or s1 is null.</exception>
        public double Distance(string s0, string s1)
        {
            if (s0 == null)
            {
                throw new ArgumentNullException(nameof(s0));
            }

            if (s1 == null)
            {
                throw new ArgumentNullException(nameof(s1));
            }

            const char special = '\n';
            int sl = s0.Length;
            int tl = s1.Length;

            if (s0.Equals(s1))
            {
                return 0;
            }

            if (sl == 0 || tl == 0)
            {
                return 1;
            }

            int cost = 0;
            if (sl < n || tl < n)
            {
                for (int i1 = 0, ni = Math.Min(sl, tl); i1 < ni; i1++)
                {
                    if (s0[i1] == s1[i1])
                    {
                        cost++;
                    }
                }
                return (float)cost / Math.Max(sl, tl);
            }

            char[] sa = new char[sl + n - 1];
            float[] p; // 'previous' cost array, horizontally
            float[] d; // Cost array, horizontally
            // SSNET removed unneeded: float[] d2; // Placeholder to assist in swapping p and d

            // Construct sa with prefix
            for (int i1 = 0; i1 < sa.Length; i1++)
            {
                if (i1 < n - 1)
                {
                    sa[i1] = special; // Add prefix
                }
                else
                {
                    sa[i1] = s0[i1 - n + 1];
                }
            }
            p = new float[sl + 1];
            d = new float[sl + 1];

            // Indexes into strings s and t
            int i; // Iterates through source
            int j; // Iterates through target

            char[] t_j = new char[n]; // jth n-gram of t

            for (i = 0; i <= sl; i++)
            {
                p[i] = i;
            }

            for (j = 1; j <= tl; j++)
            {
                // Construct t_j n-gram 
                if (j < n)
                {
                    for (int ti = 0; ti < n - j; ti++)
                    {
                        t_j[ti] = special; // Add prefix
                    }
                    for (int ti = n - j; ti < n; ti++)
                    {
                        t_j[ti] = s1[ti - (n - j)];
                    }
                }
                else
                {
                    t_j = s1.Substring(j - n, n).ToCharArray();
                }
                d[0] = j;
                for (i = 1; i <= sl; i++)
                {
                    cost = 0;
                    int tn = n;
                    // Compare sa to t_j
                    for (int ni = 0; ni < n; ni++)
                    {
                        if (sa[i - 1 + ni] != t_j[ni])
                        {
                            cost++;
                        }
                        else if (sa[i - 1 + ni] == special)
                        { 
                            // Discount matches on prefix
                            tn--;
                        }
                    }
                    float ec = (float)cost / tn;
                    // Minimum of cell to the left+1, to the top+1, diagonally left and up +cost
                    d[i] = Math.Min(Math.Min(d[i - 1] + 1, p[i] + 1), p[i - 1] + ec);
                }
                // Copy current distance counts to 'previous row' distance counts
                (p, d) = (d, p); // SSNET specific: swap p and d using tuples
            }

            // Our last action in the above loop was to switch d and p, so p now
            // actually has the most recent cost counts
            return p[sl] / Math.Max(tl, sl);
        }
    }
}
