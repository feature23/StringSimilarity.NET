using System;
using System.Collections.Generic;
using F23.StringSimilarity.Interfaces;

namespace F23.StringSimilarity
{
    /// <summary>
    /// Ratcliff/Obershelp pattern recognition
    /// 
    /// The Ratcliff/Obershelp algorithm computes the similarity of two strings a
    /// the doubled number of matching characters divided by the total number of
    /// characters in the two strings.Matching characters are those in the longest
    /// common subsequence plus, recursively, matching characters in the unmatched
    /// region on either side of the longest common subsequence.
    /// The Ratcliff/Obershelp distance is computed as 1 - Ratcliff/Obershelp
    /// similarity.
    ///
    /// Author: Ligi https://github.com/dxpux (as a patch for fuzzystring)
    /// Ported to java from .net by denmase
    /// Ported back to .NET by paulirwin to retain compatibility with upstream Java project
    /// </summary>
    public class RatcliffObershelp : INormalizedStringSimilarity, INormalizedStringDistance
    {
        /// <summary>
        /// Compute the Ratcliff-Obershelp similarity between strings.
        /// </summary>
        /// <param name="s1">The first string to compare.</param>
        /// <param name="s2">The second string to compare.</param>
        /// <returns>The RatcliffObershelp similarity in the range [0, 1]</returns>
        /// <exception cref="System.ArgumentNullException">If s1 or s2 is null.</exception>
        public double Similarity(string s1, string s2)
        {
            if (s1 == null)
            {
                throw new ArgumentNullException(nameof(s1), "s1 must not be null");
            }

            if (s2 == null)
            {
                throw new ArgumentNullException(nameof(s2), "s2 must not be null");
            }

            if (s1.Equals(s2))
            {
                return 1.0d;
            }

            var matches = GetMatchList(s1.AsSpan(), s2.AsSpan());
            int sumOfMatches = 0;

            foreach (var match in matches)
            {
                sumOfMatches += match.Length;
            }

            return 2.0d * sumOfMatches / (s1.Length + s2.Length);
        }

        /// <summary>
        /// Return 1 - similarity.
        /// </summary>
        /// <param name="s1">The first string to compare.</param>
        /// <param name="s2">The second string to compare.</param>
        /// <returns>1 - similarity</returns>
        /// <exception cref="System.ArgumentNullException">If s1 or s2 is null.</exception>
        public double Distance(string s1, string s2)
        {
            return 1.0d - Similarity(s1, s2);
        }

        private static IList<string> GetMatchList(ReadOnlySpan<char> s1, ReadOnlySpan<char> s2)
        {
            var list = new List<string>();
            var match = FrontMaxMatch(s1, s2);

            if (match.Length > 0)
            {
                var frontSource = s1.Slice(0, s1.IndexOf(match, StringComparison.Ordinal));
                var frontTarget = s2.Slice(0, s2.IndexOf(match, StringComparison.Ordinal));
                var frontQueue = GetMatchList(frontSource, frontTarget);

                var endSource = s1.Slice(s1.IndexOf(match, StringComparison.Ordinal) + match.Length);
                var endTarget = s2.Slice(s2.IndexOf(match, StringComparison.Ordinal) + match.Length);
                var endQueue = GetMatchList(endSource, endTarget);

                list.Add(match.ToString());
                list.AddRange(frontQueue);
                list.AddRange(endQueue);
            }

            return list;
        }

        private static ReadOnlySpan<char> FrontMaxMatch(ReadOnlySpan<char> s1, ReadOnlySpan<char> s2)
        {
            int longest = 0;
            ReadOnlySpan<char> longestSubstring = ReadOnlySpan<char>.Empty;

            for (int i = 0; i < s1.Length; ++i)
            {
                for (int j = i + 1; j <= s1.Length; ++j)
                {
                    var substring = s1.Slice(i, j - i);
                    if (s2.Contains(substring, StringComparison.Ordinal) && substring.Length > longest)
                    {
                        longest = substring.Length;
                        longestSubstring = substring;
                    }
                }
            }

            return longestSubstring;
        }
    }
}