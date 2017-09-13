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
    public class QGramTest
    {
        [Fact]
        public void TestDistance()
        {
            var instance = new QGram(k: 2);

            // AB BC CD CE
            // 1  1  1  0
            // 1  1  0  1
            // Total: 2

            var result = instance.Distance("ABCD", "ABCE");

            Assert.Equal(expected: 2.0, actual: result);

            Assert.Equal(
                expected: 0.0,
                actual: instance.Distance("S", "S"),
                precision: 1); // 0.0

            Assert.Equal(
                expected: 0.0,
                actual: instance.Distance("012345", "012345"),
                precision: 1); // 0.0

            // NOTE: not using null/empty tests in NullEmptyTests because QGram is different
            Assert.Equal(0.0, instance.Distance("", ""), 1);
            Assert.Equal(2.0, instance.Distance("", "foo"), 1);
            Assert.Equal(2.0, instance.Distance("foo", ""), 1);

            NullEmptyTests.AssertArgumentNullExceptions(instance);
        }
    }
}
