using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StringSimilarity.Tests
{
    [TestClass]
    public class DamerauTests
    {
        [TestMethod]
        public void TestDistance()
        {
            var damerau = new Damerau();

            Assert.AreEqual(1.0, damerau.Distance("ABCDEF", "ABDCEF"), 0.0);
            Assert.AreEqual(2.0, damerau.Distance("ABCDEF", "BACDFE"), 0.0);
            Assert.AreEqual(1.0, damerau.Distance("ABCDEF", "ABCDE"), 0.0);
        }
    }
}
