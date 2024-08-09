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
using System.Diagnostics.CodeAnalysis;
using F23.StringSimilarity.Tests.TestUtil;
using Xunit;

namespace F23.StringSimilarity.Tests
{
    [SuppressMessage("ReSharper", "ArgumentsStyleLiteral")]
    [SuppressMessage("ReSharper", "ArgumentsStyleNamedExpression")]
    [SuppressMessage("ReSharper", "ArgumentsStyleOther")]
    public class JaroWinklerTest
    {
        [InlineData("My string", "My tsring", 0.974074)]
        [InlineData("My string", "My ntrisg", 0.896296)]
        [Theory]
        public void TestSimilarity(string s1, string s2, double expected)
        {
            var instance = new JaroWinkler();

            // test string version
            Assert.Equal(
                expected,
                actual: instance.Similarity(s1, s2),
                precision: 6 // 0.000001
            );
            
            // test char span version
            Assert.Equal(
                expected,
                actual: instance.Similarity(s1.AsSpan(), s2.AsSpan()),
                precision: 6 // 0.000001
            );
            
            // test byte span version
            Assert.Equal(
                expected,
                actual: instance.Similarity<byte>(
                    EncodingUtil.Latin1.GetBytes(s1).AsSpan(),
                    EncodingUtil.Latin1.GetBytes(s2).AsSpan()),
                precision: 6 // 0.000001
            );
        }

        [Fact]
        public void NullEmptyDistanceTest()
        {
            var instance = new JaroWinkler();
            NullEmptyTests.TestDistance(instance);

            // TODO: regular (non-null/empty) distance tests
        }
        
        [Fact]
        public void NullEmptySimilarityTest()
        {
            var instance = new JaroWinkler();
            NullEmptyTests.TestSimilarity(instance);
        }
    }
}
