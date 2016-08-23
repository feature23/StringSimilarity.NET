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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F23.StringSimilarity.Utils
{
    public class SparseIntegerVector
    {
        public int[] Keys { get; protected set; }
        public int[] Values { get; protected set; }
        public int Size { get; protected set; } = 0;

        public SparseIntegerVector() : this(20) { }

        public SparseIntegerVector(int size)
        {
            Keys = new int[size];
            Values = new int[size];
        }

        public SparseIntegerVector(IDictionary<int, int> dictionary) : this(dictionary.Count)
        {
            foreach (var key in dictionary.Keys.OrderBy(k => k))
            {
                Keys[Size] = key;
                Values[Size] = dictionary[key];
                Size++;
            }
        }

        public SparseIntegerVector(int[] array)
        {
            Size = array.Count(i => i != 0);

            Keys = new int[Size];
            Values = new int[Size];
            
            int j = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != 0)
                {
                    Keys[j] = i;
                    Values[j] = array[i];
                    j++;
                }
            }
        }

        public double CosineSimilarity(SparseIntegerVector other)
        {
            double den = this.Norm() * other.Norm();
            double agg = 0;
            int i = 0;
            int j = 0;
            while (i < Keys.Length && j < other.Keys.Length)
            {
                int k1 = Keys[i];
                int k2 = other.Keys[j];

                if (k1 == k2)
                {
                    agg += Values[i] * other.Values[j] / den;
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

        public double DotProduct(SparseIntegerVector other)
        {
            double agg = 0;
            int i = 0;
            int j = 0;
            while (i < Keys.Length && j < other.Keys.Length)
            {
                int k1 = Keys[i];
                int k2 = other.Keys[j];

                if (k1 == k2)
                {
                    agg += this.Values[i] * other.Values[j];
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

        public double DotProduct(double[] other)
        {
            double agg = 0;
            for (int i = 0; i < Keys.Length; i++)
            {
                agg += other[Keys[i]] * Values[i];
            }
            return agg;
        }

        public double Norm()
        {
            double agg = 0;
            for (int i = 0; i < Values.Length; i++)
            {
                agg += Values[i] * Values[i];
            }
            return Math.Sqrt(agg);
        }

        public double Jaccard(SparseIntegerVector other)
        {
            int intersection = this.Intersection(other);
            return (double)intersection / (Size + other.Size - intersection);
        }

        public int Union(SparseIntegerVector other)
        {
            return Size + other.Size - this.Intersection(other);
        }

        public int Intersection(SparseIntegerVector other)
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

        public double QGram(SparseIntegerVector other)
        {
            double agg = 0;
            int i = 0, j = 0;
            int k1, k2;

            while (i < Keys.Length && j < other.Keys.Length)
            {
                k1 = Keys[i];
                k2 = other.Keys[j];

                if (k1 == k2)
                {
                    agg += Math.Abs(Values[i] - other.Values[j]);
                    i++;
                    j++;

                }
                else if (k1 < k2)
                {
                    agg += Math.Abs(Values[i]);
                    i++;

                }
                else
                {
                    agg += Math.Abs(other.Values[j]);
                    j++;
                }
            }

            // Maybe one of the two vectors was not completely walked...
            while (i < Keys.Length)
            {
                agg += Math.Abs(Values[i]);
                i++;
            }

            while (j < other.Keys.Length)
            {
                agg += Math.Abs(other.Values[j]);
                j++;
            }
            return agg;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < Size; i++)
            {
                sb.Append($"{Keys[i]}:{Values[i]} ");
            }

            return sb.ToString().Trim();
        }
    }
}
