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
using Xunit;

namespace F23.StringSimilarity.Tests
{
    [SuppressMessage("ReSharper", "ArgumentsStyleLiteral")]
    [SuppressMessage("ReSharper", "ArgumentsStyleNamedExpression")]
    [SuppressMessage("ReSharper", "ArgumentsStyleOther")]
    public class OptimalStringAlignmentTest
    {
        [Fact]
        public void TestDistance()
        {
            var instance = new OptimalStringAlignment();

            // zero length
            Assert.Equal(
                expected: 6.0,
                actual: instance.Distance("", "ABCDEF"),
                precision: 0 // 0.0
            );
            Assert.Equal(
                expected: 6.0,
                actual: instance.Distance("ABCDEF", ""),
                precision: 0 // 0.0
            );
            Assert.Equal(
                expected: 0.0,
                actual: instance.Distance("", ""),
                precision: 0 // 0.0
            );

            // equality
            Assert.Equal(
                expected: 0.0,
                actual: instance.Distance("ABCDEF", "ABCDEF"),
                precision: 0 // 0.0
            );

            // single operation
            Assert.Equal(
                expected: 1.0,
                actual: instance.Distance("ABDCFE", "ABDCEF"),
                precision: 0 // 0.0
            );
            Assert.Equal(
                expected: 1.0,
                actual: instance.Distance("BBDCEF", "ABDCEF"),
                precision: 0 // 0.0
            );
            Assert.Equal(
                expected: 1.0,
                actual: instance.Distance("BDCEF", "ABDCEF"),
                precision: 0 // 0.0
            );
            Assert.Equal(
                expected: 1.0,
                actual: instance.Distance("ABDCEF", "ADCEF"),
                precision: 0 // 0.0
            );

            // other
            Assert.Equal(
                expected: 3.0,
                actual: instance.Distance("CA", "ABC"),
                precision: 0 // 0.0
            );
            Assert.Equal(
                expected: 2.0,
                actual: instance.Distance("BAC", "CAB"),
                precision: 0 // 0.0
            );
            Assert.Equal(
                expected: 4.0,
                actual: instance.Distance("abcde", "awxyz"),
                precision: 0 // 0.0
            );
            Assert.Equal(
                expected: 5.0,
                actual: instance.Distance("abcde", "vwxyz"),
                precision: 0 // 0.0
            );
        }
    }
}
