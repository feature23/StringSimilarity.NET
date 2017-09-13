/*
 * The MIT License
 *
 * Copyright 2016 feature[23]
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

namespace F23.StringSimilarity
{
    /// Used to indicate the cost of character substitution.
    ///
    /// Cost should always be in [0.0 .. 1.0]
    /// For example, in an OCR application, cost('o', 'a') could be 0.4
    /// In a checkspelling application, cost('u', 'i') could be 0.4 because these are
    /// next to each other on the keyboard...
    public interface ICharacterSubstitution
    {
        /// <summary>
        /// Indicate the cost of substitution c1 and c2.
        /// </summary>
        /// <param name="c1">The first character of the substitution.</param>
        /// <param name="c2">The second character of the substitution.</param>
        /// <returns>The cost in the range [0, 1].</returns>
        double Cost(char c1, char c2);
    }
}
