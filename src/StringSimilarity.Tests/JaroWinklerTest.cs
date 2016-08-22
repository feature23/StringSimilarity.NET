using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StringSimilarity.Tests
{
    [TestClass]
    public class JaroWinklerTest
    {
        [TestMethod]
        public void TestSimilarity()
        {
            var instance = new JaroWinkler();

            Assert.AreEqual(
                expected: 0.974074,
                actual: instance.Similarity("My string", "My tsring"),
                delta: 0.000001
            );

            Assert.AreEqual(
                expected: 0.896296,
                actual: instance.Similarity("My string", "My ntrisg"),
                delta: 0.000001
            );
        }
    }
}
