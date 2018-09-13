/*
 * The MIT License
 *
 * Copyright 2017 feature[23]
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
using F23.StringSimilarity.Interfaces;
using Xunit;

namespace F23.StringSimilarity.Tests.TestUtil
{
    public static class NullEmptyTests
    {
        public static void TestDistance(INormalizedStringDistance instance)
        {
            Assert.Equal(0.0, instance.Distance("", ""), 1);
            Assert.Equal(1.0, instance.Distance("", "foo"), 1);
            Assert.Equal(1.0, instance.Distance("foo", ""), 1);

            AssertArgumentNullExceptions(instance);
        }

        public static void TestDistance(IStringDistance instance)
        {
            Assert.Equal(0.0, instance.Distance("", ""), 1);
            Assert.Equal(3.0, instance.Distance("", "foo"), 1);
            Assert.Equal(3.0, instance.Distance("foo", ""), 1);

            AssertArgumentNullExceptions(instance);
        }

        public static void TestSimilarity(INormalizedStringSimilarity instance)
        {
            Assert.Equal(1.0, instance.Similarity("", ""), 1);
            Assert.Equal(0.0, instance.Similarity("", "foo"), 1);
            Assert.Equal(0.0, instance.Similarity("foo", ""), 1);

            Assert.Throws<ArgumentNullException>(() => instance.Similarity(null, null));
            Assert.Throws<ArgumentNullException>(() => instance.Similarity(null, ""));
            Assert.Throws<ArgumentNullException>(() => instance.Similarity("", null));
        }

        public static void AssertArgumentNullExceptions(IStringDistance instance)
        {
            Assert.Throws<ArgumentNullException>(() => instance.Distance(null, null));
            Assert.Throws<ArgumentNullException>(() => instance.Distance(null, ""));
            Assert.Throws<ArgumentNullException>(() => instance.Distance("", null));
        }
    }
}
