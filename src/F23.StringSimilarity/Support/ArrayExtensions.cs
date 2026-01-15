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

using System;
using System.Linq;

namespace F23.StringSimilarity.Support
{
    /// <summary>
    /// Provides extension methods for working with arrays.
    /// </summary>
    /// <remarks>This class contains utility methods that extend the functionality of arrays, enabling
    /// additional operations such as creating a padded version of an array. These methods are designed to simplify
    /// common array manipulation tasks.</remarks>
    internal static class ArrayExtensions
    {
        /// <summary>
        /// Creates a new array by padding the source array to the specified final length with the given padding value.
        /// </summary>
        /// <param name="source">The source array to be padded.</param>
        /// <param name="finalLength">The desired final length of the array after padding. Must be greater than or equal to the length of <paramref name="source"/>.</param>
        /// <param name="paddingValue">The value used to pad the array if it is shorter than <paramref name="finalLength"/>.</param>
        /// <returns>A new array of length <paramref name="finalLength"/> containing the elements of <paramref name="source"/>, followed by padding values if necessary.</returns>
        /// <param name="source">The source array to be padded.</param>
        /// <param name="finalLength">The desired final length of the array after padding. Must be greater than or equal to the length of <paramref name="source"/>.</param>
        /// <param name="paddingValue">The value used to pad the array if it is shorter than <paramref name="finalLength"/>.</param>
        /// <returns>A new array of length <paramref name="finalLength"/> containing the elements of <paramref name="source"/>, followed by padding values if necessary.</returns>
        internal static T[] WithPadding<T>(this T[] source, int finalLength, T paddingValue = default(T))
        {
            if (finalLength < source.Length)
                throw new InvalidOperationException("Final length must be greater than or equal to current length.");

            if (finalLength == source.Length)
                return source;

            var result = new T[finalLength];
            var padding = Enumerable.Repeat(paddingValue, finalLength - source.Length).ToArray();

            Array.Copy(source, sourceIndex: 0, destinationArray: result, destinationIndex: 0, length: source.Length);
            Array.Copy(padding, sourceIndex: 0, destinationArray: result, destinationIndex: source.Length, length: padding.Length);

            return result;
        }
    }
}
