# String Similarity .NET

[![.NET](https://github.com/feature23/StringSimilarity.NET/actions/workflows/dotnet.yml/badge.svg)](https://github.com/feature23/StringSimilarity.NET/actions/workflows/dotnet.yml)

A .NET port of java-string-similarity:
<https://github.com/tdebatty/java-string-similarity>

A library implementing different string similarity and distance measures. A dozen of algorithms (including Levenshtein edit distance and sibblings, Jaro-Winkler, Longest Common Subsequence, cosine similarity etc.) are currently implemented. Check the summary table below for the complete list...

* [Download](#download)
* [Overview](#overview)
* [Normalized, metric, similarity and distance](#normalized-metric-similarity-and-distance)
* [Shingles (n-gram) based similarity and distance](#shingles-n-gram-based-similarity-and-distance)
* [Levenshtein](#levenshtein)
* [Normalized Levenshtein](#normalized-levenshtein)
* [Weighted Levenshtein](#weighted-levenshtein)
* [Damerau-Levenshtein](#damerau-levenshtein)
* [Jaro-Winkler](#jaro-winkler)
* [Longest Common Subsequence](#longest-common-subsequence)
* [Metric Longest Common Subsequence](#metric-longest-common-subsequence)
* [N-Gram](#n-gram)
* [Shingle (n-gram) based algorithms](#shingle-n-gram-based-algorithms)
  * [Q-Gram](#shingle-n-gram-based-algorithms)
  * [Cosine similarity](#shingle-n-gram-based-algorithms)
  * [Jaccard index](#shingle-n-gram-based-algorithms)
  * [Sorensen-Dice coefficient](#shingle-n-gram-based-algorithms)
* [Ratcliff-Obershelp](#ratcliff-obershelp)
* [Experimental](#experimental)
  * [SIFT4](#sift4)
* [License](#license)

## Download
Using NuGet:
```
Install-Package F23.StringSimilarity
```

## Overview

The main characteristics of each implemented algorithm are presented below. The "cost" column gives an estimation of the computational cost to compute the similarity between two strings of length m and n respectively.

|  									|  						| Normalized? 	| Metric?	| Type    | Cost |
|--------							|-------				|-------------	|----------	| ------  | ---- |
| [Levenshtein](#levenshtein)		|distance 				| No 			| Yes 		|         | O(m*n) <sup>1</sup> |
| [Normalized Levenshtein](#normalized-levenshtein)	|distance<br>similarity	| Yes 			| No 		| 	      | O(m*n) <sup>1</sup> |
| [Weighted Levenshtein](#weighted-levenshtein)		|distance 				| No 			| No 		| 	      | O(m*n) <sup>1</sup> |
| [Damerau-Levenshtein](#damerau-levenshtein) <sup>3</sup> 	|distance 				| No 			| Yes 		| 	      | O(m*n) <sup>1</sup> |
| Optimal String Alignment <sup>3</sup> |not implemented yet | No 			| No 		| 	      | O(m*n) <sup>1</sup> |
| [Jaro-Winkler](#jaro-winkler) 		|similarity<br>distance	| Yes  			| No 		| 	      | O(m*n) |
| [Longest Common Subsequence](#longest-common-subsequence) 		|distance 				| No 			| No 		| 	      | O(m*n) <sup>1,2</sup> |
| [Metric Longest Common Subsequence](#metric-longest-common-subsequence) |distance   			| Yes 			| Yes  		| 	      | O(m*n) <sup>1,2</sup> |
| [N-Gram](#n-gram)	 				|distance				| Yes  			| No 		| 	      | O(m*n) |
| [Q-Gram](#q-gram) 				|distance  			 	| No  			| No 		| Profile | O(m+n) |
| [Cosine similarity](#cosine-similarity) 				|similarity<br>distance | Yes  			| No  		| Profile | O(m+n) |
| [Jaccard index](#jaccard-index)				|similarity<br>distance | Yes  			| Yes  		| Set	  | O(m+n) |
| [Sorensen-Dice coefficient](#sorensen-dice-coefficient) 	|similarity<br>distance | Yes 			| No 		| Set	  | O(m+n) |
| [Ratcliff-Obershelp](#ratcliff-obershelp) |similarity<br>distance | Yes | No | | ? | |

[1] In this library, Levenshtein edit distance, LCS distance and their sibblings are computed using the **dynamic programming** method, which has a cost O(m.n). For Levenshtein distance, the algorithm is sometimes called **Wagner-Fischer algorithm** ("The string-to-string correction problem", 1974). The original algorithm uses a matrix of size m x n to store the Levenshtein distance between string prefixes.

If the alphabet is finite, it is possible to use the **method of four russians** (Arlazarov et al. "On economic construction of the transitive closure of a directed graph", 1970) to speedup computation. This was published by Masek in 1980 ("A Faster Algorithm Computing String Edit Distances"). This method splits the matrix in blocks of size t x t. Each possible block is precomputed to produce a lookup table. This lookup table can then be used to compute the string similarity (or distance) in O(nm/t). Usually, t is choosen as log(m) if m > n. The resulting computation cost is thus O(mn/log(m)). This method has not been implemented (yet).

[2] In "Length of Maximal Common Subsequences", K.S. Larsen proposed an algorithm that computes the length of LCS in time O(log(m).log(n)). But the algorithm has a memory requirement O(m.n²) and was thus not implemented here.

[3] There are two variants of Damerau-Levenshtein string distance: Damerau-Levenshtein with adjacent transpositions (also sometimes called unrestricted Damerau–Levenshtein distance) and Optimal String Alignment (also sometimes called restricted edit distance). For Optimal String Alignment, no substring can be edited more than once.

## Normalized, metric, similarity and distance
Although the topic might seem simple, a lot of different algorithms exist to measure text similarity or distance. Therefore the library defines some interfaces to categorize them.

### (Normalized) similarity and distance

- StringSimilarity : Implementing algorithms define a similarity between strings (0 means strings are completely different).
- NormalizedStringSimilarity : Implementing algorithms define a similarity between 0.0 and 1.0, like Jaro-Winkler for example.
- StringDistance : Implementing algorithms define a distance between strings (0 means strings are identical), like Levenshtein for example. The maximum distance value depends on the algorithm.
- NormalizedStringDistance : This interface extends StringDistance. For implementing classes, the computed distance value is between 0.0 and 1.0. NormalizedLevenshtein is an example of NormalizedStringDistance.

Generally, algorithms that implement NormalizedStringSimilarity also implement NormalizedStringDistance, and similarity = 1 - distance. But there are a few exceptions, like N-Gram similarity and distance (Kondrak)...

### Metric distances
The MetricStringDistance interface : A few of the distances are actually metric distances, which means that verify the triangle inequality d(x, y) <= d(x,z) + d(z,y). For example, Levenshtein is a metric distance, but NormalizedLevenshtein is not.

A lot of nearest-neighbor search algorithms and indexing structures rely on the triangle inequality. You can check "Similarity Search, The Metric Space Approach" by Zezula et al. for a survey. These cannot be used with non metric similarity measures.

<!--[Read Javadoc for a detailed description](http://api123.web-d.be/api/java-string-similarity/head/index.html)-->
<!-- TODO.JB - API documentation link? -->

## Shingles (n-gram) based similarity and distance
A few algorithms work by converting strings into sets of n-grams (sequences of n characters, also sometimes called k-shingles). The similarity or distance between the strings is then the similarity or distance between the sets.

Some ot them, like jaccard, consider strings as sets of shingles, and don't consider the number of occurences of each shingle. Others, like cosine similarity, work using what is sometimes called the profile of the strings, which takes into account the number of occurences of each shingle.

For these algorithms, another use case is possible when dealing with large datasets:
1. compute the set or profile representation of all the strings
2. compute the similarity between sets or profiles

## Levenshtein
The Levenshtein distance between two words is the minimum number of single-character edits (insertions, deletions or substitutions) required to change one word into the other.

It is a metric string distance. This implementation uses dynamic programming (Wagner–Fischer algorithm), with only 2 rows of data. The space requirement is thus O(m) and the algorithm runs in O(m.n).

```cs
using System;
using F23.StringSimilarity;

public class Program
{    
    public static void Main(string[] args)
    {
        var l = new Levenshtein();

        Console.WriteLine(l.Distance("My string", "My $tring"));
        Console.WriteLine(l.Distance("My string", "My $tring"));
        Console.WriteLine(l.Distance("My string", "My $tring"));
    }    
}
```

## Normalized Levenshtein
This distance is computed as levenshtein distance divided by the length of the longest string. The resulting value is always in the interval [0.0 1.0] but it is not a metric anymore!

The similarity is computed as 1 - normalized distance.

```cs
using System;
using F23.StringSimilarity;

public class Program
{    
    public static void Main(string[] args)
    {
        var l = new NormalizedLevenshtein();

        Console.WriteLine(l.Distance("My string", "My $tring"));
        Console.WriteLine(l.Distance("My string", "My $tring"));
        Console.WriteLine(l.Distance("My string", "My $tring"));
    }
}
```

## Weighted Levenshtein
An implementation of Levenshtein that allows to define different weights for different character substitutions.

This algorithm is usually used for optical character recognition (OCR) applications. For OCR, the cost of substituting P and R is lower then the cost of substituting P and M for example because because from and OCR point of view P is similar to R.

It can also be used for keyboard typing auto-correction. Here the cost of substituting E and R is lower for example because these are located next to each other on an AZERTY or QWERTY keyboard. Hence the probability that the user mistyped the characters is higher.

```cs
using System;
using F23.StringSimilarity;

public class Program
{    
    public static void Main(string[] args)
    {
        var l = new WeightedLevenshtein(new ExampleCharSub());

        Console.WriteLine(l.Distance("String1", "String1"));
        Console.WriteLine(l.Distance("String1", "Srring1"));
        Console.WriteLine(l.Distance("String1", "Srring2"));
    }
}

private class ExampleCharSub : ICharacterSubstitution
{
    public double Cost(char c1, char c2)
    {
        // The cost for substituting 't' and 'r' is considered smaller as these 2 are located next to each other on a keyboard
        if (c1 == 't' && c2 == 'r') return 0.5; 

        // For most cases, the cost of substituting 2 characters is 1.0
        return 1.0;
    }
}
```

## Damerau-Levenshtein
Similar to Levenshtein, Damerau-Levenshtein distance with transposition (also sometimes calls unrestricted Damerau-Levenshtein distance) is the minimum number of operations needed to transform one string into the other, where an operation is defined as an insertion, deletion, or substitution of a single character, or a **transposition of two adjacent characters**.

It does respect triangle inequality, and is thus a metric distance.

This is not to be confused with the optimal string alignment distance, which is an extension where no substring can be edited more than once.

```cs
using System;
using F23.StringSimilarity;

public class Program
{
    public static void Main(string[] args)
    {
        var d = new Damerau();
        
        // 1 substitution
        Console.WriteLine(d.Distance("ABCDEF", "ABDCEF"));
        
        // 2 substitutions
        Console.WriteLine(d.Distance("ABCDEF", "BACDFE"));
        
        // 1 deletion
        Console.WriteLine(d.Distance("ABCDEF", "ABCDE"));
        Console.WriteLine(d.Distance("ABCDEF", "BCDEF"));
        Console.WriteLine(d.Distance("ABCDEF", "ABCGDEF"));
        
        // All different
        Console.WriteLine(d.Distance("ABCDEF", "POIU"));
    }    
}
```

Will produce:

```
1.0
2.0
1.0
1.0
1.0
6.0
```



## Jaro-Winkler
Jaro-Winkler is a string edit distance that was developed in the area of record linkage (duplicate detection) (Winkler, 1990). The Jaro–Winkler distance metric is designed and best suited for short strings such as person names, and to detect transposition typos.

Jaro-Winkler computes the similarity between 2 strings, and the returned value lies in the interval [0.0, 1.0].
It is (roughly) a variation of Damerau-Levenshtein, where the transposition of 2 close characters is considered less important than the transposition of 2 characters that are far from each other. Jaro-Winkler penalizes additions or substitutions that cannot be expressed as transpositions.

The distance is computed as 1 - Jaro-Winkler similarity.

```cs
using System;
using F23.StringSimilarity;

public class Program
{
    public static void Main(string[] args)
    {
        var jw = new JaroWinkler();
        
        // substitution of s and t
        Console.WriteLine(jw.Similarity("My string", "My tsring"));
        
        // substitution of s and n
        Console.WriteLine(jw.Similarity("My string", "My ntrisg"));
    }
}
```

will produce:

```
0.9740740656852722
0.8962963223457336
```

## Longest Common Subsequence

The longest common subsequence (LCS) problem consists in finding the longest subsequence common to two (or more) sequences. It differs from problems of finding common substrings: unlike substrings, subsequences are not required to occupy consecutive positions within the original sequences.

It is used by the diff utility, by Git for reconciling multiple changes, etc.

The LCS distance between strings X (of length n) and Y (of length m) is n + m - 2 |LCS(X, Y)|
min = 0
max = n + m

LCS distance is equivalent to Levenshtein distance when only insertion and deletion is allowed (no substitution), or when the cost of the substitution is the double of the cost of an insertion or deletion.

This class implements the dynamic programming approach, which has a space requirement O(m.n), and computation cost O(m.n).

In "Length of Maximal Common Subsequences", K.S. Larsen proposed an algorithm that computes the length of LCS in time O(log(m).log(n)). But the algorithm has a memory requirement O(m.n²) and was thus not implemented here.

```cs
using System;
using F23.StringSimilarity;

public class Program
{
    public static void Main(string[] args)
    {
        var lcs = new LongestCommonSubsequence();

        // Will produce 4.0
        Console.WriteLine(lcs.Distance("AGCAT", "GAC"));
        
        // Will produce 1.0
        Console.WriteLine(lcs.Distance("AGCAT", "AGCT"));
    }
}
```

## Metric Longest Common Subsequence
Distance metric based on Longest Common Subsequence, from the notes "An LCS-based string metric" by Daniel Bakkelund.
http://heim.ifi.uio.no/~danielry/StringMetric.pdf

The distance is computed as 1 - |LCS(s1, s2)| / max(|s1|, |s2|)

```cs
using System;
using F23.StringSimilarity;

public class Program
{
    public static void Main(string[] args)
    {
        var lcs = new MetricLCS();

        string s1 = "ABCDEFG";   
        string s2 = "ABCDEFHJKL";
        // LCS: ABCDEF => length = 6
        // longest = s2 => length = 10
        // => 1 - 6/10 = 0.4
        Console.WriteLine(lcs.Distance(s1, s2));

        // LCS: ABDF => length = 4
        // longest = ABDEF => length = 5
        // => 1 - 4 / 5 = 0.2
        Console.WriteLine(lcs.Distance("ABDEF", "ABDIF"));
    }
}
```

## N-Gram

Normalized N-Gram distance as defined by Kondrak, "N-Gram Similarity and Distance", String Processing and Information Retrieval, Lecture Notes in Computer Science Volume 3772, 2005, pp 115-126.

http://webdocs.cs.ualberta.ca/~kondrak/papers/spire05.pdf

The algorithm uses affixing with special character '\n' to increase the weight of first characters. The normalization is achieved by dividing the total similarity score the original length of the longest word.

In the paper, Kondrak also defines a similarity measure, which is not implemented (yet).

```cs
using System;
using F23.StringSimilarity;

public class Program
{
    public static void Main(string[] args)
    {   
        // produces 0.583333
        var twogram = new NGram(2);
        Console.WriteLine(twogram.Distance("ABCD", "ABTUIO"));
        
        // produces 0.97222
        string s1 = "Adobe CreativeSuite 5 Master Collection from cheap 4zp";
        string s2 = "Adobe CreativeSuite 5 Master Collection from cheap d1x";
        var ngram = new NGram(4);
        Console.WriteLine(ngram.Distance(s1, s2));
    }
}
```

## Shingle (n-gram) based algorithms
A few algorithms work by converting strings into sets of n-grams (sequences of n characters, also sometimes called k-shingles). The similarity or distance between the strings is then the similarity or distance between the sets.

The cost for computing these similarities and distances is mainly domnitated by k-shingling (converting the strings into sequences of k characters). Therefore there are typically two use cases for these algorithms:

Directly compute the distance between strings:

```cs
using System;
using F23.StringSimilarity;

public class Program
{
    public static void Main(string[] args)
    {
        var dig = new QGram(2);
        
        // AB BC CD CE
        // 1  1  1  0
        // 1  1  0  1
        // Total: 2

        Console.WriteLine(dig.Distance("ABCD", "ABCE"));
    }
}
```

Or, for large datasets, pre-compute the profile or set representation of all strings. The similarity can then be computed between profiles or sets:

```cs
using System;
using F23.StringSimilarity;

public class Program
{
    public static void Main(string[] args)
    {
        string s1 = "My first string";
        string s2 = "My other string...";
        
        // Let's work with sequences of 2 characters...
        var cosine = new Cosine(2);
        
        // For cosine similarity I need the profile of strings
        var profile1 = cosine.GetProfile(s1);
        var profile2 = cosine.GetProfile(s2);
        
        // Prints 0.516185
        Console.WriteLine(cosine.Similarity(profile1, profile2));
    }
}
```

Pay attention, this only works if the same Cosine object is used to parse all input strings!


### Q-Gram
Q-gram distance, as defined by Ukkonen in "Approximate string-matching with q-grams and maximal matches"
http://www.sciencedirect.com/science/article/pii/0304397592901434

The distance between two strings is defined as the L1 norm of the difference of their profiles (the number of occurences of each n-gram): SUM( |V1_i - V2_i| ). Q-gram distance is a lower bound on Levenshtein distance, but can be computed in O(m + n), where Levenshtein requires O(m.n)


### Cosine similarity
The similarity between the two strings is the cosine of the angle between these two vectors representation, and is computed as V1 . V2 / (|V1| * |V2|)

Distance is computed as 1 - cosine similarity.

### Jaccard index
Like Q-Gram distance, the input strings are first converted into sets of n-grams (sequences of n characters, also called k-shingles), but this time the cardinality of each n-gram is not taken into account. Each input string is simply a set of n-grams. The Jaccard index is then computed as |V1 inter V2| / |V1 union V2|.

Distance is computed as 1 - cosine similarity.
Jaccard index is a metric distance.

### Sorensen-Dice coefficient
Similar to Jaccard index, but this time the similarity is computed as 2 * |V1 inter V2| / (|V1| + |V2|).

Distance is computed as 1 - cosine similarity.

## Ratcliff-Obershelp
Ratcliff/Obershelp Pattern Recognition, also known as Gestalt Pattern Matching, is a string-matching algorithm for determining the similarity of two strings. It was developed in 1983 by John W. Ratcliff and John A. Obershelp and published in the Dr. Dobb's Journal in July 1988

Ratcliff/Obershelp computes the similarity between 2 strings, and the returned value lies in the interval [0.0, 1.0].

The distance is computed as 1 - Ratcliff/Obershelp similarity.

```cs
using System;
using F23.StringSimilarity;

public class Program
{
    public static void Main(string[] args)
    {
        var ro = new RatcliffObershelp();
        
        // substitution of s and t
        Console.WriteLine(ro.Similarity("My string", "My tsring"));
        
        // substitution of s and n
        Console.WriteLine(ro.Similarity("My string", "My ntrisg"));
    }
}
```

will produce:

```
0.8888888888888888
0.7777777777777778
```

## Experimental

### SIFT4
SIFT4 is a general purpose string distance algorithm inspired by JaroWinkler and Longest Common Subsequence. It was developed to produce a distance measure that matches as close as possible to the human perception of string distance. Hence it takes into account elements like character substitution, character distance, longest common subsequence etc. It was developed using experimental testing, and without theoretical background.

```cs
using System;
using System.Diagnostics;
using F23.StringSimilarity;

public class Program
{
    public static void Main(string[] args)
    {
        var s1 = "This is the first string";
        var s2 = "And this is another string";
        var sift4 = new Sift4();
        sift4.MaxOffset = 5;
        double expResult = 11.0;
        double result = sift4.Distance(s1, s2);
        Debug.Assert(Math.Abs(result - expResult) < 0.1);
    }
}
```

## License

This code is licensed under the MIT license.
