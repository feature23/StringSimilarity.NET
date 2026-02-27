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

        [Fact]
        public void ExampleFromReadme()
        {
            // produces 0.416666
            var twogram = new NGram(2);
            Assert.Equal(
                expected: 0.417,
                actual: twogram.Distance("ABCD", "ABTUIO"),
                precision: 3);

            // produces 0.97222
            string s1 = "Adobe CreativeSuite 5 Master Collection from cheap 4zp";
            string s2 = "Adobe CreativeSuite 5 Master Collection from cheap d1x";
            var ngram = new NGram(4);
            Assert.Equal(
                expected: 0.972,
                actual: ngram.Distance(s1, s2),
                precision: 3);
        }
    }
}
