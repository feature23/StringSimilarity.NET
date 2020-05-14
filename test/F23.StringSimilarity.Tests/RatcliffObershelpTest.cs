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

using System.Diagnostics.CodeAnalysis;
using F23.StringSimilarity.Tests.TestUtil;
using Xunit;

namespace F23.StringSimilarity.Tests
{
    [SuppressMessage("ReSharper", "ArgumentsStyleLiteral")]
    [SuppressMessage("ReSharper", "ArgumentsStyleNamedExpression")]
    [SuppressMessage("ReSharper", "ArgumentsStyleOther")]
    public class RatcliffObershelpTest
    {
        [Fact]
        public void TestSimilarity()
        {
            var instance = new RatcliffObershelp();

            /// test data from other algorithms
            /// "My string" vs "My tsring"
            /// Substrings:
            /// "ring" ==> 4, "My s" ==> 3, "s" ==> 1
            /// Ratcliff-Obershelp = 2*(sum of substrings)/(length of s1 + length of s2)
            ///                    = 2*(4 + 3 + 1) / (9 + 9)
            ///                    = 16/18
            ///                    = 0.888888888888889
            Assert.Equal(
                expected: 0.888888888888889,
                actual: instance.Similarity("My string", "My tsring"),
                precision: 15 /// 0.000000000000001
            );

            /// test data from other algorithms
            /// "My string" vs "My tsring"
            /// Substrings:
            /// "My " ==> 3, "tri" ==> 3, "g" ==> 1
            /// Ratcliff-Obershelp = 2*(sum of substrings)/(length of s1 + length of s2)
            ///                    = 2*(3 + 3 + 1) / (9 + 9)
            ///                    = 14/18
            ///                    = 0.777777777777778
            Assert.Equal(
                expected: 0.777777777777778,
                actual: instance.Similarity("My string", "My ntrisg"),
                precision: 15 /// 0.000000000000001
            );

            /// test data from essay by Ilya Ilyankou
            /// "Comparison of Jaro-Winkler and Ratcliff/Obershelp algorithms
            /// in spell check"
            /// https://ilyankou.files.wordpress.com/2015/06/ib-extended-essay.pdf
            /// p13, expected result is 0.857
            Assert.Equal(
                expected: 0.857,
                actual: instance.Similarity("MATEMATICA", "MATHEMATICS"),
                precision: 3 /// 0.001
            );

            /// test data from stringmetric
            /// https://github.com/rockymadden/stringmetric
            /// expected output is 0.736842105263158
            Assert.Equal(
                expected: 0.736842105263158,
                actual: instance.Similarity("aleksander", "alexandre"),
                precision: 15 /// 0.000000000000001
            );

            /// test data from stringmetric
            /// https://github.com/rockymadden/stringmetric
            /// expected output is 0.666666666666667
            Assert.Equal(
                expected: 0.666666666666667,
                actual: instance.Similarity("pennsylvania", "pencilvaneya"),
                precision: 15 /// 0.000000000000001
            );

            /// test data from wikipedia
            /// https://en.wikipedia.org/wiki/Gestalt_Pattern_Matching
            /// expected output is 14/18 = 0.777777777777778‬
            Assert.Equal(
                expected: 0.777777777777778,
                actual: instance.Similarity("WIKIMEDIA", "WIKIMANIA"),
                precision: 15 /// 0.000000000000001
            );

            /// test data from wikipedia
            /// https://en.wikipedia.org/wiki/Gestalt_Pattern_Matching
            /// expected output is 24/40 = 0.6
            Assert.Equal(
                expected: 0.6,
                actual: instance.Similarity("GESTALT PATTERN MATCHING", "GESTALT PRACTICE"),
                precision: 15 /// 0.000000000000001
            );

            NullEmptyTests.TestSimilarity(instance);
        }

        [Fact]
        public void TestDistance()
        {
            var instance = new RatcliffObershelp();
            NullEmptyTests.TestDistance(instance);

            /// TODO: regular (non-null/empty) distance tests
        }
    }
}
