using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringSimilarity.Support;

namespace StringSimilarity.Tests.Support
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
