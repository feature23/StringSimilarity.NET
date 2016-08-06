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
using StringSimilarity.Interfaces;

namespace StringSimilarity
{
    /// <summary>
    /// Implementation of Damerau-Levenshtein distance with transposition (also 
    /// sometimes calls unrestricted Damerau-Levenshtein distance).
    /// It is the minimum number of operations needed to transform one string into
    /// the other, where an operation is defined as an insertion, deletion, or
    /// substitution of a single character, or a transposition of two adjacent
    /// characters.
    /// It does respect triangle inequality, and is thus a metric distance.
    /// 
    /// This is not to be confused with the optimal string alignment distance, which
    /// is an extension where no substring can be edited more than once.
    /// </summary>
    public class Damerau : IMetricStringDistance, IStringDistance
    {
        public double Distance(string s1, string s2)
        {
            // Infinite distance is the max possible distance
            int INFINITE = s1.Length + s2.Length;

            // Create and initialize the character array indices
            var DA = new Dictionary<char, int>();

            for (int d = 0; d < s1.Length; d++)
            {
                if (!DA.ContainsKey(s1[d]))
                {
                    DA[s1[d]] = 0;
                }
            }

            for (int d = 0; d < s2.Length; d++)
            {
                if (!DA.ContainsKey(s2[d]))
                {
                    DA[s2[d]] = 0;
                }
            }

            // Create the distance matrix H[0 .. s1.length+1][0 .. s2.length+1]
            int[,] H = new int[s1.Length + 2, s2.Length + 2];

            // Initialize the left and top edges of H
            for (int i = 0; i <= s1.Length; i++)
            {
                H[i + 1, 0] = INFINITE;
                H[i + 1, 1] = i;
            }

            for (int j = 0; j <= s2.Length; j++)
            {
                H[0, j + 1] = INFINITE;
                H[1, j + 1] = j;
            }

            // Fill in the distance matrix H
            // Look at each character in s1
            for (int i = 1; i <= s1.Length; i++)
            {
                int DB = 0;

                // Look at each character in b
                for (int j = 1; j <= s2.Length; j++)
                {
                    int i1 = DA[s2[j - 1]];
                    int j1 = DB;

                    int cost = 1;
                    if (s1[i - 1] == s2[j - 1])
                    {
                        cost = 0;
                        DB = j;
                    }

                    H[i + 1, j + 1] = Min(
                        H[i, j] + cost,  // Substitution
                        H[i + 1, j] + 1, // Insertion
                        H[i, j + 1] + 1, // Deletion
                        H[i1, j1] + (i - i1 - 1) + 1 + (j - j1 - 1)
                    );
                }

                DA[s1[i - 1]] = i;
            }

            return H[s1.Length + 1, s2.Length + 1];
        }

        protected static int Min(int a, int b, int c, int d)
             => Math.Min(a, Math.Min(b, Math.Min(c, d)));
    }
}
