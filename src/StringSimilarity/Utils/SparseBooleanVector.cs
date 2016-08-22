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

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringSimilarity.Utils
{
    public class SparseBooleanVector
    {
        public int[] Keys { get; protected set; }
        public int Size => Keys.Length;

        public SparseBooleanVector() : this(20) { }

        public SparseBooleanVector(int size)
        {
            Keys = new int[size];
        }

        public SparseBooleanVector(IDictionary<int, int> dictionary) 
            : this(dictionary.Count)
        {
            int size = 0;

            foreach (var key in dictionary.Keys.OrderBy(k => k))
            {
                Keys[size] = key;
                size++;
            }
        }

        public SparseBooleanVector(bool[] array)
        {

            int size = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i])
                {
                    size++;
                }
            }

            Keys = new int[size];
            int j = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i])
                {
                    Keys[j] = i;
                    j++;
                }
            }
        }

        public double Jaccard(SparseBooleanVector other)
        {
            int intersection = this.Intersection(other);
            return (double)intersection / (this.Size + other.Size - intersection);
        }

        public int Union(SparseBooleanVector other)
        {
            return this.Size + other.Size - this.Intersection(other);
        }

        public int Intersection(SparseBooleanVector other)
        {
            int agg = 0;
            int i = 0;
            int j = 0;

            while (i < Keys.Length && j < other.Keys.Length)
            {
                int k1 = Keys[i];
                int k2 = other.Keys[j];

                if (k1 == k2)
                {
                    agg++;
                    i++;
                    j++;
                }
                else if (k1 < k2)
                {
                    i++;

                }
                else
                {
                    j++;
                }
            }

            return agg;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < Size; i++)
            {
                sb.Append($"{Keys[i]}:{Keys[i]} ");
            }

            return sb.ToString().Trim();
        }
    }
}
