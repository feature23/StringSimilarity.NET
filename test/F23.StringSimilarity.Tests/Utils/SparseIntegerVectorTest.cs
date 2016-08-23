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

using F23.StringSimilarity.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace F23.StringSimilarity.Tests.Utils
{
    [TestClass]
    public class SparseIntegerVectorTest
    {
        [TestMethod]
        public void TestDotProduct()
        {
            var other = new SparseIntegerVector(new int[] { 0, 2, 0, 1 });
            var instance = new SparseIntegerVector(new int[] { 1, 2, 1, 0 });

            double expResult = 4.0;
            double result = instance.DotProduct(other);

            Assert.AreEqual(expResult, result, 0.0);
        }

        [TestMethod]
        public void TestDotProduct_DoubleArray()
        {
            double[] other = new double[] { 0, 1.5, 2.0, 3.0 };
            var instance = new SparseIntegerVector(new int[] { 1, 2, 0, 0 });

            double expResult = 3.0;
            double result = instance.DotProduct(other);

            Assert.AreEqual(expResult, result, 0.0);
        }

        [TestMethod]
        public void TestCosineSimilarity()
        {
            var other = new SparseIntegerVector(new int[] { 0, 1, 2, 3 });
            var instance = new SparseIntegerVector(new int[] { 1, 2, 0, 0 });

            double expResult = instance.DotProduct(other) / (instance.Norm() * other.Norm());
            double result = instance.CosineSimilarity(other);

            Assert.AreEqual(expResult, result, 0.0);
        }

        [TestMethod]
        public void TestNorm()
        {
            var instance = new SparseIntegerVector(new int[] { 0, 0, 2 });

            double expResult = 2.0;
            double result = instance.Norm();

            Assert.AreEqual(expResult, result, 0.0);
        }
    }
}
