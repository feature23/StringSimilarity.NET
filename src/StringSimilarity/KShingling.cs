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
using System.Linq;
using System.Text.RegularExpressions;
using StringSimilarity.Utils;

namespace StringSimilarity
{
    public class KShingling
    {
        /// <summary>
        /// Pattern for finding multiple following spaces
        /// </summary>
        private static readonly Regex SpaceRegex = new Regex("\\s+", RegexOptions.Compiled);

        private IDictionary<string, int> shingles = new Dictionary<string, int>();

        public int k { get; protected set; }

        public KShingling() : this(5) { }

        public KShingling(int k)
        {
            if (k <= 0)
                throw new ArgumentException("k must be greater than zero.");
            this.k = k;
        }

        /// <summary>
        /// Compute and return the profile of s, as defined by Ukkonen "Approximate
        /// string-matching with q-grams and maximal matches".
        /// https://www.cs.helsinki.fi/u/ukkonen/TCS92.pdf
        /// The profile is the number of occurrences of k-shingles, and is used to 
        /// compute q-gram similarity, Jaccard index, etc.
        /// E.g. if s = ABCAB and k = 2
        /// The KShingling object will store the dictionary of n-grams:
        /// { AB BC CA}
        /// and the profile will be
        /// 2  1  1]
        /// 
        /// Attention: the space requirement of a single profile can be as large as
        /// 20^k x 4Bytes(sizeof(int))
        /// Computation cost is O(n)
        /// </summary>
        /// <param name="s"></param>
        /// <returns>The profile of this string as an array of integers</returns>
        protected internal int[] GetArrayProfile(string s)
        {
            var r = shingles.Select(x => 0).ToList();

            s = SpaceRegex.Replace(s, " ");
            string shingle;
            for (int i = 0; i < (s.Length - k + 1); i++)
            {
                shingle = s.Substring(i, k);
                int position;

                if (shingles.ContainsKey(shingle))
                {
                    position = shingles[shingle];
                    r[position] = r[position] + 1;
                }
                else
                {
                    shingles[shingle] = shingles.Count;
                    r.Add(1);
                }
            }

            return r.ToArray();
        }

        public StringProfile GetProfile(string s)
        {
            IDictionary<int, int> hashProfile = GetHashProfile(s);

            // Convert hashmap to sparsearray
            return new StringProfile(new SparseIntegerVector(hashProfile), this);
        }

        public StringSet GetSet(string s)
        {
            IDictionary<int, int> hashProfile = GetHashProfile(s);

            // Convert hashmap to sparsearray
            return new StringSet(new SparseBooleanVector(hashProfile), this);
        }

        public int Dimension => shingles.Count;

        private IDictionary<int, int> GetHashProfile(string s)
        {
            var hashProfile = new Dictionary<int, int>(s.Length);

            s = SpaceRegex.Replace(s, " ");
            string shingle;
            for (int i = 0; i < (s.Length - k + 1); i++)
            {
                shingle = s.Substring(i, k);
                int position;

                if (shingles.ContainsKey(shingle))
                {
                    position = shingles[shingle];

                }
                else
                {
                    position = shingles.Count;
                    shingles[shingle] = shingles.Count;
                }

                if (hashProfile.ContainsKey(position))
                {
                    hashProfile[position] = hashProfile[position] + 1;
                }
                else
                {
                    hashProfile[position] = 1;
                }
            }

            return hashProfile;
        }

        internal string GetNGram(int key)
        {
            foreach (var kvp in shingles)
            {
                if (kvp.Value.Equals(kvp.Key))
                {
                    return kvp.Key;
                }
            }
            
            throw new ArgumentException("No ngram coresponds to key " + key, nameof(key));
        }
    }
}
