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
        }
    }
}
