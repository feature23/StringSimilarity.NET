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
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace F23.StringSimilarity
{
    public abstract class ShingleBased
    {
        private const int DEFAULT_K = 3;

        /// <summary>
        /// Return k, the length of k-shingles (aka n-grams).
        /// </summary>
        protected int k { get; }

        /// <summary>
        /// Pattern for finding multiple following spaces
        /// </summary>
        private static readonly Regex SPACE_REG = new Regex("\\s+", RegexOptions.Compiled);

        /// <summary> 
        /// </summary>
        /// <param name="k"></param>
        /// <exception cref="ArgumentOutOfRangeException">If k is less than or equal to 0.</exception>
        protected ShingleBased(int k)
        {
            if (k <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(k), "k should be positive!");
            }

            this.k = k;
        }
        
        protected ShingleBased() : this(DEFAULT_K) { }

        protected internal Dictionary<string, int> GetProfile(string s)
        {
            var shingles = new Dictionary<string, int>();

            var string_no_space = SPACE_REG.Replace(s, " ");

            for (int i = 0; i < (string_no_space.Length - k + 1); i++)
            {
                var shingle = string_no_space.Substring(i, k);

                if (shingles.TryGetValue(shingle, out var old))
                {
                    shingles[shingle] = old + 1;
                }
                else
                {
                    shingles[shingle] = 1;
                }
            }

            return shingles;
        }
    }
}
