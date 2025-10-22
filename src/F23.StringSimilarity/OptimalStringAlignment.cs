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
// ReSharper disable SuggestVarOrType_Elsewhere
// ReSharper disable TooWideLocalVariableScope

namespace F23.StringSimilarity
{
    public sealed class OptimalStringAlignment : IStringDistance, ISpanDistance
    {
        /// <summary>
        /// Compute the distance between strings: the minimum number of operations
        /// needed to transform one string into the other (insertion, deletion,
        /// substitution of a single character, or a transposition of two adjacent
        /// characters) while no substring is edited more than once.
        /// </summary>
        /// <param name="s1">The first string to compare.</param>
        /// <param name="s2">The second string to compare.</param>
        /// <returns>the OSA distance</returns>
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
                return 0;
            }

            int n = s1.Length, m = s2.Length;

            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Create the distance matrix H[0 .. s1.length+1][0 .. s2.length+1]
            int[,] d = new int[n + 2, m + 2];

            //initialize top row and leftmost column
            for (int i = 0; i <= n; i++)
            {
                d[i, 0] = i;
            }
            for (int j = 0; j <= m; j++)
            {
                d[0, j] = j;
            }

            //fill the distance matrix
            int cost;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    //if s1[i - 1] = s2[j - 1] then cost = 0, else cost = 1
                    cost = 1;

                    if (s1[i - 1].Equals(s2[j - 1]))
                    {
                        cost = 0;
                    }

                    d[i, j] = Min(
                            d[i - 1, j - 1] + cost, // substitution
                            d[i, j - 1] + 1,        // insertion
                            d[i - 1, j] + 1         // deletion
                    );

                    //transposition check
                    if (i > 1 && j > 1
                            && s1[i - 1].Equals(s2[j - 2])
                            && s1[i - 2].Equals(s2[j - 1])
                        )
                    {
                        d[i, j] = Math.Min(d[i, j], d[i - 2, j - 2] + cost);
                    }
                }
            }

            return d[n, m];
        }

        private static int Min(int a, int b, int c)
            => Math.Min(a, Math.Min(b, c));
    }
}
