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

namespace F23.StringSimilarity.Experimental
{
    /// <summary>
    /// Sift4 - a general purpose string distance algorithm inspired by JaroWinkler
    /// and Longest Common Subsequence.
    /// Original JavaScript algorithm by siderite, java port by Nathan Fischer 2016.
    /// https://siderite.dev/blog/super-fast-and-accurate-string-distance.html
    /// https://blackdoor.github.io/blog/sift4-java/
    /// </summary>
    public class Sift4 : IStringDistance
    {
        private const int DEFAULT_MAX_OFFSET = 10;

        /// <summary>
        /// Gets or sets the maximum distance to search for character transposition.
        /// Compuse cost of algorithm is O(n . MaxOffset)
        /// </summary>
        public int MaxOffset { get; set; } = DEFAULT_MAX_OFFSET;

        /// <summary>
        /// Used to store relation between same character in different positions
        /// c1 and c2 in the input strings.
        /// </summary>
        /// <remarks>
        /// .NET port notes: should this be a struct instead?
        /// </remarks>
        private class Offset
        {
            internal readonly int c1;
            internal readonly int c2;
            internal bool trans;

            internal Offset(int c1, int c2, bool trans)
            {
                this.c1 = c1;
                this.c2 = c2;
                this.trans = trans;
            }
        }

        /// <summary>
        /// Sift4 - a general purpose string distance algorithm inspired by JaroWinkler
        /// and Longest Common Subsequence.
        /// Original JavaScript algorithm by siderite, java port by Nathan Fischer 2016.
        /// https://siderite.dev/blog/super-fast-and-accurate-string-distance.html
        /// https://blackdoor.github.io/blog/sift4-java/
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public double Distance(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1))
            {
                if (s2 == null)
                {
                    return 0;
                }

                return s2.Length;
            }

            if (string.IsNullOrEmpty(s2))
            {
                return s1.Length;
            }

            int l1 = s1.Length;
            int l2 = s2.Length;

            int c1 = 0;  //cursor for string 1
            int c2 = 0;  //cursor for string 2
            int lcss = 0;  //largest common subsequence
            int local_cs = 0; //local common substring
            int trans = 0;  //number of transpositions ('ab' vs 'ba')

            // offset pair array, for computing the transpositions
            var offset_arr = new List<Offset>();

            while ((c1 < l1) && (c2 < l2))
            {
                if (s1[c1] == s2[c2])
                {
                    local_cs++;
                    bool is_trans = false;
                    // see if current match is a transposition
                    int i = 0;
                    while (i < offset_arr.Count)
                    {
                        Offset ofs = offset_arr[i];
                        if (c1 <= ofs.c1 || c2 <= ofs.c2)
                        {
                            // when two matches cross, the one considered a
                            // transposition is the one with the largest difference
                            // in offsets
                            is_trans = Math.Abs(c2 - c1) >= Math.Abs(ofs.c2 - ofs.c1);

                            if (is_trans)
                            {
                                trans++;
                            }
                            else
                            {
                                if (!ofs.trans)
                                {
                                    ofs.trans = true;
                                    trans++;
                                }
                            }

                            break;
                        }
                        else
                        {
                            if (c1 > ofs.c2 && c2 > ofs.c1)
                            {
                                offset_arr.RemoveAt(i);
                            }
                            else
                            {
                                i++;
                            }
                        }
                    }

                    offset_arr.Add(new Offset(c1, c2, is_trans));
                }
                else
                {
                    // s1.charAt(c1) != s2.charAt(c2)
                    lcss += local_cs;
                    local_cs = 0;
                    if (c1 != c2)
                    {
                        //using min allows the computation of transpositions
                        c1 = Math.Min(c1, c2);
                        c2 = c1;
                    }

                    // if matching characters are found, remove 1 from both cursors
                    // (they get incremented at the end of the loop)
                    // so that we can have only one code block handling matches
                    for (int i = 0;
                         i < MaxOffset && (c1 + i < l1 || c2 + i < l2);
                         i++)
                    {
                        if ((c1 + i < l1) && (s1[c1 + i] == s2[c2]))
                        {
                            c1 += i - 1;
                            c2--;
                            break;
                        }

                        if ((c2 + i < l2) && (s1[c1] == s2[c2 + i]))
                        {
                            c1--;
                            c2 += i - 1;
                            break;
                        }
                    }
                }
                c1++;
                c2++;
                // this covers the case where the last match is on the last token
                // in list, so that it can compute transpositions correctly
                if ((c1 >= l1) || (c2 >= l2))
                {
                    lcss += local_cs;
                    local_cs = 0;
                    c1 = Math.Min(c1, c2);
                    c2 = c1;
                }
            }

            lcss += local_cs;

            // add the cost of transpositions to the final result
            return Math.Round((double) (Math.Max(l1, l2) - lcss + trans));
        }
    }
}
