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
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable SuggestVarOrType_Elsewhere

namespace F23.StringSimilarity
{
    /// <summary>
    /// Implementation of Damerau-Levenshtein distance with transposition (also 
    /// sometimes calls unrestricted Damerau-Levenshtein distance).
    /// It is the minimum number of operations needed to transform one string into
    /// the other, where an operation is defined as an insertion, deletion, or
    /// substitution of a single character, or a transposition of two adjacent
    /// characters.
    /// It does respect triangle inequality, and is thus a metric distance.
    /// This is not to be confused with the optimal string alignment distance, which
    /// is an extension where no substring can be edited more than once.
    /// </summary>
    public class Damerau : IMetricStringDistance, IMetricSpanDistance
    {
        /// <summary>
        /// Compute the distance between strings: the minimum number of operations
        /// needed to transform one string into the other(insertion, deletion,
        /// substitution of a single character, or a transposition of two adjacent
        /// characters).
        /// </summary>
        /// <param name="s1">The first string to compare.</param>
        /// <param name="s2">The second string to compare.</param>
        /// <returns>The computed distance.</returns>
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

            // Infinite distance is the max possible distance
            int inf = s1.Length + s2.Length;

            // Create and initialize the character array indices
            var da = new Dictionary<T, int>();

            for (int d = 0; d < s1.Length; d++)
            {
                da[s1[d]] = 0;
            }

            for (int d = 0; d < s2.Length; d++)
            {
                da[s2[d]] = 0;
            }

            // Create the distance matrix H[0 .. s1.length+1][0 .. s2.length+1]
            int[,] h = new int[s1.Length + 2, s2.Length + 2];

            // Initialize the left and top edges of H
            for (int i = 0; i <= s1.Length; i++)
            {
                h[i + 1, 0] = inf;
                h[i + 1, 1] = i;
            }

            for (int j = 0; j <= s2.Length; j++)
            {
                h[0, j + 1] = inf;
                h[1, j + 1] = j;
            }

            // Fill in the distance matrix H
            // Look at each character in s1
            for (int i = 1; i <= s1.Length; i++)
            {
                int db = 0;

                // Look at each character in b
                for (int j = 1; j <= s2.Length; j++)
                {
                    int i1 = da[s2[j - 1]];
                    int j1 = db;

                    int cost = 1;
                    if (s1[i - 1].Equals(s2[j - 1]))
                    {
                        cost = 0;
                        db = j;
                    }

                    h[i + 1, j + 1] = Min(
                        h[i, j] + cost,  // Substitution
                        h[i + 1, j] + 1, // Insertion
                        h[i, j + 1] + 1, // Deletion
                        h[i1, j1] + (i - i1 - 1) + 1 + (j - j1 - 1)
                    );
                }

                da[s1[i - 1]] = i;
            }

            return h[s1.Length + 1, s2.Length + 1];
        }

        private static int Min(int a, int b, int c, int d)
             => Math.Min(a, Math.Min(b, Math.Min(c, d)));
    }
}
