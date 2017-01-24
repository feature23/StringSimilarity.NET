using F23.StringSimilarity.Tests.TestUtil;
using Xunit;

namespace F23.StringSimilarity.Tests
{
    public class NGramTest
    {
        [Fact]
        public void TestDistance()
        {
            var s0 = "ABABABAB";
            var s1 = "ABCABCABCABC";
            var s2 = "POIULKJH";

            var ngram = new NGram();

            Assert.True(ngram.Distance(s0, s1) < ngram.Distance(s0, s2));

            Assert.Equal(
                expected: 0.0,
                actual: ngram.Distance("SIJK", "SIJK"),
                precision: 1); // 0.0

            Assert.Equal(
                expected: 0.0,
                actual: ngram.Distance("S", "S"),
                precision: 1); // 0.0

            Assert.Equal(
                expected: 1.0,
                actual: ngram.Distance("", "S"),
                precision: 1); // 0.0

            Assert.Equal(
                expected: 1.0,
                actual: ngram.Distance("", "SIJK"),
                precision: 1); // 0.0

            NullEmptyTests.TestDistance(ngram);
        }
    }
}
