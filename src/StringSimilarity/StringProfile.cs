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
using StringSimilarity.Utils;

namespace StringSimilarity
{
    /// <summary>
    /// Profile of a string (number of occurences of each shingle/n-gram), computed
    /// using shingling.
    /// </summary>
    public class StringProfile
    {
        public SparseIntegerVector Vector { get; private set; }
        private readonly KShingling _ks;

        public StringProfile(SparseIntegerVector vector, KShingling ks)
        {
            Vector = vector;
            _ks = ks;
        }

        public double CosineSimilarity(StringProfile other)
        {
            if (_ks != other._ks)
            {
                throw new Exception("Profiles were not created using the same kshingling object!");
            }

            return Vector.CosineSimilarity(other.Vector);
        }

        public double QGramDistance(StringProfile other)
        {
            if (_ks != other._ks)
            {
                throw new Exception("Profiles were not created using the same kshingling object!");
            }

            return Vector.QGram(other.Vector);
        }

        public string[] getMostFrequentNGrams(int number)
        {
            string[] strings = new string[number];
            int[] frequencies = new int[number];

            int position_smallest_frequency = 0;

            for (int i = 0; i < Vector.Size; i++)
            {
                int key = Vector.Keys[i];
                int frequency = Vector.Values[i];
                string ngram = _ks.GetNGram(key);

                if (frequency > frequencies[position_smallest_frequency])
                {
                    // 1. Replace the element with currently the smallest frequency
                    strings[position_smallest_frequency] = ngram;
                    frequencies[position_smallest_frequency] = frequency;

                    // 2. Loop over frequencies to find which one is now the lowest
                    // frequency
                    int smallest_frequency = int.MaxValue;
                    for (int j = 0; j < frequencies.Length; j++)
                    {
                        if (frequencies[j] < smallest_frequency)
                        {
                            position_smallest_frequency = j;
                            smallest_frequency = frequencies[j];
                        }
                    }
                }
            }

            return strings;
        }
    }
}
