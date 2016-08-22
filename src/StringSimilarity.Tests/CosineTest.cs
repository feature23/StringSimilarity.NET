using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StringSimilarity.Tests
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
