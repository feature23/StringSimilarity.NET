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

using System.Linq;
using F23.StringSimilarity.Support;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace F23.StringSimilarity.Tests.Support
{
    [TestClass]
    public class ArrayExtensionsTest
    {
        [TestMethod]
        public void TestWithPadding()
        {
            var source = Enumerable.Repeat(42, 1000).ToArray();

            var padded = source.WithPadding(1200);

            Assert.AreEqual(1200, padded.Length);
            Assert.IsTrue(padded.Take(1000).All(x => x == 42));
            Assert.IsTrue(padded.Skip(1000).Take(200).All(x => x == 0));
        }
    }
}
