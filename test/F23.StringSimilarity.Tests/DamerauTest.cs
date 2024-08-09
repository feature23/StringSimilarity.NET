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
    public class DamerauTest
    {
        [InlineData("ABCDEF", "ABDCEF", 1.0)]
        [InlineData("ABCDEF", "BACDFE", 2.0)]
        [InlineData("ABCDEF", "ABCDE", 1.0)]
        [Theory]
        public void TestDistance(string s1, string s2, double expected)
        {
            var instance = new Damerau();

            // test string version
            Assert.Equal(expected, actual: instance.Distance(s1, s2));
            
            // test char span version
            Assert.Equal(expected, actual: instance.Distance(s1.AsSpan(), s2.AsSpan()));

            // test byte span version
            Assert.Equal(expected, actual: instance.Distance<byte>(
                EncodingUtil.Latin1.GetBytes(s1).AsSpan(), 
                EncodingUtil.Latin1.GetBytes(s2).AsSpan()));
        }
        
        [Fact]
        public void NullEmptyDistanceTest()
        {
            var instance = new Damerau();
            NullEmptyTests.TestDistance(instance);
        }
    }
}
