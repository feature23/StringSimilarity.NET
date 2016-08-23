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

using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace F23.StringSimilarity.Tests
{
    [TestClass]
    public class CosineTest
    {
        [TestMethod]
        public void TestSimilarity()
        {
            var instance = new Cosine();

            double result = instance.Similarity("ABC", "ABCE");

            Assert.AreEqual(0.71, result, 0.01);
        }

        [TestMethod]
        public void TestSmallString()
        {
            var instance = new Cosine(3);

            double result = instance.Similarity("AB", "ABCE");

            Assert.AreEqual(0.0, result, 0.00001);
        }

        [TestMethod]
        public async Task TestLargeString()
        {
            var instance = new Cosine();

            // Read text from two files
            var string1 = await ReadResourceFileAsync("71816-2.txt");
            var string2 = await ReadResourceFileAsync("11328-1.txt");

            double result = instance.Similarity(string1, string2);

            Assert.AreEqual(0.8115, result, 0.001);
        }

        private static async Task<string> ReadResourceFileAsync(string file)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"{typeof(CosineTest).Namespace}.{file}";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
