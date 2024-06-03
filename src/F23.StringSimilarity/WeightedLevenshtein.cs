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
using System.Threading;
using F23.StringSimilarity.Interfaces;
// ReSharper disable SuggestVarOrType_Elsewhere
// ReSharper disable TooWideLocalVariableScope
// ReSharper disable IntroduceOptionalParameters.Global

namespace F23.StringSimilarity
{
    /// Implementation of Levenshtein that allows to define different weights for
    /// different character substitutions.
    public class WeightedLevenshtein : IStringDistance
    {
        private readonly ICharacterSubstitution _characterSubstitution;
        private readonly ICharacterInsDel _characterInsDel;

        /// <summary>
        /// Instantiate with provided character substitution.
        /// </summary>
        /// <param name="characterSubstitution">The strategy to determine character substitution weights.</param>
        public WeightedLevenshtein(ICharacterSubstitution characterSubstitution)
            : this(characterSubstitution, null)
        {
        }

        /// <summary>
        /// Instantiate with provided character substitution, insertion, and
        /// deletion weights.
        /// </summary>
        /// <param name="characterSubstitution">The strategy to determine character substitution weights.</param>
        /// <param name="characterInsDel">The strategy to determine character insertion/deletion weights.</param>
        public WeightedLevenshtein(ICharacterSubstitution characterSubstitution,
            ICharacterInsDel characterInsDel)
        {
            _characterSubstitution = characterSubstitution;
            _characterInsDel = characterInsDel;
        }

        /// <summary>
        /// Equivalent to Distance(s1, s2, Double.MaxValue).
        /// </summary>
        /// <param name="s1">The first string to compare.</param>
        /// <param name="s2">The second string to compare.</param>
        /// <returns>The computed weighted Levenshtein distance.</returns>
        public double Distance(string s1, string s2)
        {
            return Distance(s1, s2, double.MaxValue);
        }

        /// <summary>
        /// Compute Levenshtein distance using provided weights for substitution.
        /// </summary>
        /// <param name="s1">The first string to compare.</param>
        /// <param name="s2">The second string to compare.</param>
        /// <param name="limit">The maximum result to compute before stopping. This
        /// means that the calculation can terminate early if you
        /// only care about strings with a certain similarity.
        /// Set this to Double.MaxValue if you want to run the
        /// calculation to completion in every case.</param>
        /// <returns>The computed weighted Levenshtein distance.</returns>
        /// <exception cref="ArgumentNullException">If s1 or s2 is null.</exception>
        public double Distance(string s1, string s2, double limit)
        {
            if (s1 == null)
            {
                throw new ArgumentNullException(nameof(s1));
            }

            if (s2 == null)
            {
                throw new ArgumentNullException(nameof(s2));
            }

            if (s1.Equals(s2))
            {
                return 0;
            }

            if (s1.Length == 0)
            {
                return s2.Length;
            }

            if (s2.Length == 0)
            {
                return s1.Length;
            }

            // create two work vectors of floating point (i.e. weighted) distances
            double[] v0 = new double[s2.Length + 1];
            double[] v1 = new double[s2.Length + 1];
            // SSNET: removed unneeded double[] vtemp;

            // initialize v0 (the previous row of distances)
            // this row is A[0][i]: edit distance for an empty s1
            // the distance is the cost of inserting each character of s2
            v0[0] = 0;
            for (int i = 1; i < v0.Length; i++)
            {
                v0[i] = v0[i - 1] + InsertionCost(s2[i - 1]);
            }

            for (int i = 0; i < s1.Length; i++)
            {
                char s1i = s1[i];
                double deletionCost = DeletionCost(s1i);

                // calculate v1 (current row distances) from the previous row v0
                // first element of v1 is A[i+1][0]
                // Edit distance is the cost of deleting characters from s1
                // to match empty t.
                v1[0] = v0[0] + deletionCost;

                double minv1 = v1[0];

                // use formula to fill in the rest of the row
                for (int j = 0; j < s2.Length; j++)
                {
                    char s2j = s2[j];
                    double cost = 0;
                    
                    if (s1i != s2j)
                    {
                        cost = _characterSubstitution.Cost(s1i, s2j);
                    }

                    double insertionCost = InsertionCost(s2j);

                    v1[j + 1] = Math.Min(
                            v1[j] + insertionCost, // Cost of insertion
                            Math.Min(
                                    v0[j + 1] + deletionCost, // Cost of deletion
                                    v0[j] + cost)); // Cost of substitution

                    minv1 = Math.Min(minv1, v1[j + 1]);
                }

                if (minv1 >= limit)
                {
                    return limit;
                }

                // copy v1 (current row) to v0 (previous row) for next iteration
                // System.arraycopy(v1, 0, v0, 0, v0.length);
                // Flip references to current and previous row
                (v0, v1) = (v1, v0); // SSNET Specific: Swap references using tuples instead of temporary
            }

            return v0[s2.Length];
        }

        private double InsertionCost(char c)
        {
            return _characterInsDel?.InsertionCost(c) ?? 1.0;
        }

        private double DeletionCost(char c)
        {
            return _characterInsDel?.DeletionCost(c) ?? 1.0;
        }
    }
}
