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
using F23.StringSimilarity.Experimental;
using Xunit;

namespace F23.StringSimilarity.Tests
{
    public class StaticMethodsTest
    {
        private const string S1 = "My string";
        private const string S2 = "My tsring";

        [Fact]
        public void TestCosine()
        {
            Assert.Equal(new Cosine().Similarity(S1, S2), Cosine.GetSimilarity(S1, S2));
            Assert.Equal(new Cosine().Distance(S1, S2), Cosine.GetDistance(S1, S2));
            Assert.Equal(new Cosine(2).Similarity(S1, S2), Cosine.GetSimilarity(S1, S2, k: 2));
            Assert.Equal(new Cosine(2).Distance(S1, S2), Cosine.GetDistance(S1, S2, k: 2));

            var profile1 = ShingleBased.GetProfile(S1, ShingleBased.DEFAULT_K);
            var profile2 = ShingleBased.GetProfile(S2, ShingleBased.DEFAULT_K);

            Assert.Equal(new Cosine().Similarity(profile1, profile2), Cosine.GetSimilarity(profile1, profile2));
        }

        [Fact]
        public void TestDamerau()
        {
            Assert.Equal(new Damerau().Distance(S1, S2), Damerau.GetDistance(S1, S2));
            Assert.Equal(new Damerau().Distance(S1.AsSpan(), S2.AsSpan()), Damerau.GetDistance(S1.AsSpan(), S2.AsSpan()));
        }

        [Fact]
        public void TestJaccard()
        {
            Assert.Equal(new Jaccard().Similarity(S1, S2), Jaccard.GetSimilarity(S1, S2));
            Assert.Equal(new Jaccard().Distance(S1, S2), Jaccard.GetDistance(S1, S2));
            Assert.Equal(new Jaccard(2).Similarity(S1, S2), Jaccard.GetSimilarity(S1, S2, k: 2));
            Assert.Equal(new Jaccard(2).Distance(S1, S2), Jaccard.GetDistance(S1, S2, k: 2));
        }

        [Fact]
        public void TestJaroWinkler()
        {
            Assert.Equal(new JaroWinkler().Similarity(S1, S2), JaroWinkler.GetSimilarity(S1, S2));
            Assert.Equal(new JaroWinkler().Distance(S1, S2), JaroWinkler.GetDistance(S1, S2));
            Assert.Equal(new JaroWinkler().Similarity(S1.AsSpan(), S2.AsSpan()), JaroWinkler.GetSimilarity(S1.AsSpan(), S2.AsSpan()));
            Assert.Equal(new JaroWinkler().Distance(S1.AsSpan(), S2.AsSpan()), JaroWinkler.GetDistance(S1.AsSpan(), S2.AsSpan()));

            Assert.Equal(new JaroWinkler(0.9).Similarity(S1, S2), JaroWinkler.GetSimilarity(S1, S2, threshold: 0.9));
            Assert.Equal(new JaroWinkler(0.9).Distance(S1, S2), JaroWinkler.GetDistance(S1, S2, threshold: 0.9));
            Assert.Equal(new JaroWinkler(0.9).Similarity(S1.AsSpan(), S2.AsSpan()), JaroWinkler.GetSimilarity(S1.AsSpan(), S2.AsSpan(), threshold: 0.9));
        }

        [Fact]
        public void TestLevenshtein()
        {
            Assert.Equal(new Levenshtein().Distance(S1, S2), Levenshtein.GetDistance(S1, S2));
            Assert.Equal(new Levenshtein().Distance(S1, S2, 1), Levenshtein.GetDistance(S1, S2, 1));
            Assert.Equal(new Levenshtein().Distance(S1.AsSpan(), S2.AsSpan()), Levenshtein.GetDistance(S1.AsSpan(), S2.AsSpan()));
            Assert.Equal(new Levenshtein().Distance(S1.AsSpan(), S2.AsSpan(), 1), Levenshtein.GetDistance(S1.AsSpan(), S2.AsSpan(), 1));
        }

        [Fact]
        public void TestLongestCommonSubsequence()
        {
            Assert.Equal(new LongestCommonSubsequence().Distance(S1, S2), LongestCommonSubsequence.GetDistance(S1, S2));
            Assert.Equal(new LongestCommonSubsequence().Distance(S1.AsSpan(), S2.AsSpan()), LongestCommonSubsequence.GetDistance(S1.AsSpan(), S2.AsSpan()));
            Assert.Equal(new LongestCommonSubsequence().Length(S1, S2), LongestCommonSubsequence.GetLength(S1, S2));
            Assert.Equal(new LongestCommonSubsequence().Length(S1, S2), LongestCommonSubsequence.GetLength(S1.AsSpan(), S2.AsSpan()));
        }

        [Fact]
        public void TestMetricLCS()
        {
            Assert.Equal(new MetricLCS().Distance(S1, S2), MetricLCS.GetDistance(S1, S2));
            Assert.Equal(new MetricLCS().Distance(S1.AsSpan(), S2.AsSpan()), MetricLCS.GetDistance(S1.AsSpan(), S2.AsSpan()));
        }

        [Fact]
        public void TestNGram()
        {
            Assert.Equal(new NGram().Distance(S1, S2), NGram.GetDistance(S1, S2));
            Assert.Equal(new NGram(3).Distance(S1, S2), NGram.GetDistance(S1, S2, n: 3));
        }

        [Fact]
        public void TestNormalizedLevenshtein()
        {
            Assert.Equal(new NormalizedLevenshtein().Distance(S1, S2), NormalizedLevenshtein.GetDistance(S1, S2));
            Assert.Equal(new NormalizedLevenshtein().Similarity(S1, S2), NormalizedLevenshtein.GetSimilarity(S1, S2));
            Assert.Equal(new NormalizedLevenshtein().Distance(S1.AsSpan(), S2.AsSpan()), NormalizedLevenshtein.GetDistance(S1.AsSpan(), S2.AsSpan()));
            Assert.Equal(new NormalizedLevenshtein().Similarity(S1.AsSpan(), S2.AsSpan()), NormalizedLevenshtein.GetSimilarity(S1.AsSpan(), S2.AsSpan()));
        }

        [Fact]
        public void TestOptimalStringAlignment()
        {
            Assert.Equal(new OptimalStringAlignment().Distance(S1, S2), OptimalStringAlignment.GetDistance(S1, S2));
            Assert.Equal(new OptimalStringAlignment().Distance(S1.AsSpan(), S2.AsSpan()), OptimalStringAlignment.GetDistance(S1.AsSpan(), S2.AsSpan()));
        }

        [Fact]
        public void TestQGram()
        {
            Assert.Equal(new QGram().Distance(S1, S2), QGram.GetDistance(S1, S2));
            Assert.Equal(new QGram(2).Distance(S1, S2), QGram.GetDistance(S1, S2, k: 2));

            var profile1 = ShingleBased.GetProfile(S1, ShingleBased.DEFAULT_K);
            var profile2 = ShingleBased.GetProfile(S2, ShingleBased.DEFAULT_K);

            Assert.Equal(new QGram().Distance(profile1, profile2), QGram.GetDistance(profile1, profile2));
        }

        [Fact]
        public void TestRatcliffObershelp()
        {
            Assert.Equal(new RatcliffObershelp().Similarity(S1, S2), RatcliffObershelp.GetSimilarity(S1, S2));
            Assert.Equal(new RatcliffObershelp().Distance(S1, S2), RatcliffObershelp.GetDistance(S1, S2));
        }

        [Fact]
        public void TestSift4()
        {
            Assert.Equal(new Sift4().Distance(S1, S2), Sift4.GetDistance(S1, S2));
            Assert.Equal(new Sift4 { MaxOffset = 5 }.Distance(S1, S2), Sift4.GetDistance(S1, S2, maxOffset: 5));
        }

        [Fact]
        public void TestSorensenDice()
        {
            Assert.Equal(new SorensenDice().Similarity(S1, S2), SorensenDice.GetSimilarity(S1, S2));
            Assert.Equal(new SorensenDice().Distance(S1, S2), SorensenDice.GetDistance(S1, S2));
            Assert.Equal(new SorensenDice(2).Similarity(S1, S2), SorensenDice.GetSimilarity(S1, S2, k: 2));
            Assert.Equal(new SorensenDice(2).Distance(S1, S2), SorensenDice.GetDistance(S1, S2, k: 2));
        }

        [Fact]
        public void TestWeightedLevenshtein()
        {
            var charSub = new ExampleCharSub();
            var insDel = new ExampleInsDel();

            Assert.Equal(new WeightedLevenshtein(charSub).Distance(S1, S2),
                WeightedLevenshtein.GetDistance(S1, S2, charSub));
            Assert.Equal(new WeightedLevenshtein(charSub).Distance(S1, S2, 1.0),
                WeightedLevenshtein.GetDistance(S1, S2, charSub, limit: 1.0));
            Assert.Equal(new WeightedLevenshtein(charSub, insDel).Distance(S1, S2),
                WeightedLevenshtein.GetDistance(S1, S2, charSub, insDel));
            Assert.Equal(new WeightedLevenshtein(charSub, insDel).Distance(S1, S2, 1.0),
                WeightedLevenshtein.GetDistance(S1, S2, charSub, insDel, 1.0));
        }

        private class ExampleCharSub : ICharacterSubstitution
        {
            public double Cost(char c1, char c2) => c1 == 't' && c2 == 'r' ? 0.5 : 1.0;
        }

        private class ExampleInsDel : ICharacterInsDel
        {
            public double DeletionCost(char c) => c == 'i' ? 0.8 : 1.0;

            public double InsertionCost(char c) => c == 'i' ? 0.5 : 1.0;
        }
    }
}
