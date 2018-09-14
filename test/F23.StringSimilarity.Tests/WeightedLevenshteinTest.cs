/*
 * The MIT License
 *
 * Copyright 2017 feature[23]
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

using F23.StringSimilarity.Tests.TestUtil;
using Xunit;

namespace F23.StringSimilarity.Tests
{
    public class WeightedLevenshteinTest
    {
        [Fact]
        public void TestDistance()
        {
            var instance = new WeightedLevenshtein(new ExampleCharSub());

            Assert.Equal(0.0, instance.Distance("String1", "String1"), 1);
            Assert.Equal(0.5, instance.Distance("String1", "Srring1"), 1);
            Assert.Equal(1.5, instance.Distance("String1", "Srring2"), 1);

            // One insert or delete.
            Assert.Equal(1.0, instance.Distance("Strng", "String"), 1);
            Assert.Equal(1.0, instance.Distance("String", "Strng"), 1);

            // With limits.
            Assert.Equal(0.0, instance.Distance("String1", "String1", double.MaxValue), 1);
            Assert.Equal(0.0, instance.Distance("String1", "String1", 2.0), 1);
            Assert.Equal(1.5, instance.Distance("String1", "Srring2", double.MaxValue), 1);
            Assert.Equal(1.5, instance.Distance("String1", "Srring2", 2.0), 1);
            Assert.Equal(1.5, instance.Distance("String1", "Srring2", 1.5), 1);
            Assert.Equal(1.0, instance.Distance("String1", "Srring2", 1.0), 1);
            Assert.Equal(4.0, instance.Distance("String1", "Potato", 4.0), 1);

            NullEmptyTests.TestDistance(instance);
        }

        [Fact]
        public void TestDistanceCharacterInsDelInterface()
        {
            var instance = new WeightedLevenshtein(new ExampleCharSub(), new ExampleInsDel());

            // Same as testDistance above.
            Assert.Equal(0.0, instance.Distance("String1", "String1"), 1);
            Assert.Equal(0.5, instance.Distance("String1", "Srring1"), 1);
            Assert.Equal(1.5, instance.Distance("String1", "Srring2"), 1);

            // Cost of insert of 'i' is less than normal, so these scores are
            // different than testDistance above.  Note that the cost of delete
            // has been set differently than the cost of insert, so the distance
            // call is not symmetric in its arguments if an 'i' has changed.
            Assert.Equal(0.5, instance.Distance("Strng", "String"), 1);
            Assert.Equal(0.8, instance.Distance("String", "Strng"), 1);
            Assert.Equal(1.0, instance.Distance("Strig", "String"), 1);
            Assert.Equal(1.0, instance.Distance("String", "Strig"), 1);

            // Same as above with limits.
            Assert.Equal(0.0, instance.Distance("String1", "String1", double.MaxValue), 1);
            Assert.Equal(0.0, instance.Distance("String1", "String1", 2.0), 1);
            Assert.Equal(1.5, instance.Distance("String1", "Srring2", double.MaxValue), 1);
            Assert.Equal(1.5, instance.Distance("String1", "Srring2", 2.0), 1);
            Assert.Equal(1.5, instance.Distance("String1", "Srring2", 1.5), 1);
            Assert.Equal(1.0, instance.Distance("String1", "Srring2", 1.0), 1);
            Assert.Equal(4.0, instance.Distance("String1", "Potato", 4.0), 1);

            NullEmptyTests.TestDistance(instance);
        }

        private class ExampleCharSub : ICharacterSubstitution
        {
            public double Cost(char c1, char c2)
            {
                // The cost for substituting 't' and 'r' is considered
                // smaller as these 2 are located next to each other
                // on a keyboard
                if (c1 == 't' && c2 == 'r')
                {
                    return 0.5;
                }

                // For most cases, the cost of substituting 2 characters
                // is 1.0
                return 1.0;
            }
        }

        private class ExampleInsDel : ICharacterInsDel
        {
            public double DeletionCost(char c)
            {
                if (c == 'i')
                {
                    return 0.8;
                }

                return 1.0;
            }

            public double InsertionCost(char c)
            {
                if (c == 'i')
                {
                    return 0.5;
                }

                return 1.0;
            }
        }
    }
}
