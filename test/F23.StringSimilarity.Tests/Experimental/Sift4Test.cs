using System;
using F23.StringSimilarity.Experimental;
using Xunit;

namespace F23.StringSimilarity.Tests.Experimental
{
    public class Sift4Test
    {
        [Fact]
        public void TestDistance()
        {
            string s1 = "This is the first string";
            string s2 = "And this is another string";

            var sift4 = new Sift4
            {
                MaxOffset = 5
            };
            
            double result = sift4.Distance(s1, s2);

            Assert.Equal(
                expected: 11.0, 
                actual: result, 
                precision: 1); // 0.0

            sift4.MaxOffset = 10;

            result = sift4.Distance(
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                "Amet Lorm ispum dolor sit amet, consetetur adixxxpiscing elit.");

            Assert.Equal(
                expected: 12.0,
                actual: result,
                precision: 1); // 0.0
        }
    }
}
