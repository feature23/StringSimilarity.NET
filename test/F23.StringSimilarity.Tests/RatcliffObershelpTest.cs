using F23.StringSimilarity.Tests.TestUtil;
using Xunit;

namespace F23.StringSimilarity.Tests
{
    public class RatcliffObershelpTest
    {
        [Fact]
        public void TestSimilarity()
        {
            var instance = new RatcliffObershelp();

            // test data from other algorithms
            // "My string" vs "My tsring"
            // Substrings:
            // "ring" ==> 4, "My s" ==> 3, "s" ==> 1
            // Ratcliff-Obershelp = 2*(sum of substrings)/(length of s1 + length of s2)
            //                    = 2*(4 + 3 + 1) / (9 + 9)
            //                    = 16/18
            //                    = 0.888888
            // NOTE.NET: actual result is 0.8888888888 repeating, but Xunit rounds to 0.888889.
            // Modified assertion from upstream Java code to reflect rounding difference between assertion APIs.
            Assert.Equal(
                    0.888889,
                    instance.Similarity("My string", "My tsring"),
                    6);

            // test data from other algorithms
            // "My string" vs "My tsring"
            // Substrings:
            // "My " ==> 3, "tri" ==> 3, "g" ==> 1
            // Ratcliff-Obershelp = 2*(sum of substrings)/(length of s1 + length of s2)
            //                    = 2*(3 + 3 + 1) / (9 + 9)
            //                    = 14/18
            //                    = 0.777778
            Assert.Equal(
                    0.777778,
                    instance.Similarity("My string", "My ntrisg"),
                    6);

            // test data from essay by Ilya Ilyankou
            // "Comparison of Jaro-Winkler and Ratcliff/Obershelp algorithms
            // in spell check"
            // https://ilyankou.files.wordpress.com/2015/06/ib-extended-essay.pdf
            // p13, expected result is 0.857
            Assert.Equal(
                    0.857,
                    instance.Similarity("MATEMATICA", "MATHEMATICS"),
                    3);

            // test data from stringmetric
            // https://github.com/rockymadden/stringmetric
            // expected output is 0.7368421052631579
            Assert.Equal(
                    0.736842,
                    instance.Similarity("aleksander", "alexandre"),
                    6);

            // test data from stringmetric
            // https://github.com/rockymadden/stringmetric
            // expected output is 0.6666666666666666
            // NOTE.NET: actual result is 0.6666666666 repeating, but Xunit rounds to 0.666667.
            // Modified assertion from upstream Java code to reflect rounding difference between assertion APIs.
            Assert.Equal(
                    0.666667,
                    instance.Similarity("pennsylvania", "pencilvaneya"),
                    6);

            // test data from wikipedia
            // https://en.wikipedia.org/wiki/Gestalt_Pattern_Matching
            // expected output is 14/18 = 0.7777777777777778
            Assert.Equal(
                    0.777778,
                    instance.Similarity("WIKIMEDIA", "WIKIMANIA"),
                    6);

            // test data from wikipedia
            // https://en.wikipedia.org/wiki/Gestalt_Pattern_Matching
            // expected output is 24/40 = 0.65
            Assert.Equal(
                    0.6,
                    instance.Similarity("GESTALT PATTERN MATCHING", "GESTALT PRACTICE"),
                    6);

            NullEmptyTests.TestSimilarity(instance);
        }

        [Fact]
        public void TestDistance()
        {
            var instance = new RatcliffObershelp();
            NullEmptyTests.TestDistance(instance);

            // TODO: regular (non-null/empty) distance tests
        }
    }
}
