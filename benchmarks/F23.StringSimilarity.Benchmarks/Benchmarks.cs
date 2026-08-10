using BenchmarkDotNet.Attributes;
using F23.StringSimilarity.Experimental;

namespace F23.StringSimilarity.Benchmarks;

[MemoryDiagnoser]
public class Benchmarks
{
    [Benchmark]
    public void Cosine()
    {
        var cosine = new Cosine();
        _ = cosine.Distance("hello", "world");
    }

    [Benchmark]
    public void Damerau()
    {
        var damerau = new Damerau();
        _ = damerau.Distance("hello", "world");
    }

    [Benchmark]
    public void Jaccard()
    {
        var jaccard = new Jaccard();
        _ = jaccard.Distance("hello", "world");
    }

    [Benchmark]
    public void JaroWinkler()
    {
        var jaro = new JaroWinkler();
        _ = jaro.Distance("hello", "world");
    }

    [Benchmark]
    public void Levenshtein()
    {
        var levenshtein = new Levenshtein();
        _ = levenshtein.Distance("hello", "world");
    }

    [Benchmark]
    public void LongestCommonSubsequence()
    {
        var lcs = new LongestCommonSubsequence();
        _ = lcs.Distance("hello", "world");
    }

    [Benchmark]
    public void MetricLCS()
    {
        var metricLcs = new MetricLCS();
        _ = metricLcs.Distance("hello", "world");
    }

    [Benchmark]
    public void NGram()
    {
        var ngram = new NGram();
        _ = ngram.Distance("hello", "world");
    }

    [Benchmark]
    public void NormalizedLevenshtein()
    {
        var normalizedLevenshtein = new NormalizedLevenshtein();
        _ = normalizedLevenshtein.Distance("hello", "world");
    }

    [Benchmark]
    public void OptimalStringAlignment()
    {
        var osa = new OptimalStringAlignment();
        _ = osa.Distance("hello", "world");
    }

    [Benchmark]
    public void QGram()
    {
        var qGram = new QGram();
        _ = qGram.Distance("hello", "world");
    }

    [Benchmark]
    public void RatcliffObershelp()
    {
        var ratcliffObershelp = new RatcliffObershelp();
        _ = ratcliffObershelp.Distance("hello", "world");
    }

    [Benchmark]
    public void Sift4()
    {
        var sift4 = new Sift4();
        _ = sift4.Distance("hello", "world");
    }

    [Benchmark]
    public void SorensenDice()
    {
        var sorensenDice = new SorensenDice();
        _ = sorensenDice.Distance("hello", "world");
    }

    [Benchmark]
    public void WeightedLevenshtein()
    {
        var weightedLevenshtein = new WeightedLevenshtein(new ExampleCharSub());
        _ = weightedLevenshtein.Distance("hello", "world");
    }

#if STATIC_METHODS
    [Benchmark]
    public void CosineStatic() => _ = F23.StringSimilarity.Cosine.GetDistance("hello", "world");

    [Benchmark]
    public void DamerauStatic() => _ = F23.StringSimilarity.Damerau.GetDistance("hello", "world");

    [Benchmark]
    public void JaccardStatic() => _ = F23.StringSimilarity.Jaccard.GetDistance("hello", "world");

    [Benchmark]
    public void JaroWinklerStatic() => _ = F23.StringSimilarity.JaroWinkler.GetDistance("hello", "world");

    [Benchmark]
    public void LevenshteinStatic() => _ = F23.StringSimilarity.Levenshtein.GetDistance("hello", "world");

    [Benchmark]
    public void LongestCommonSubsequenceStatic() => _ = F23.StringSimilarity.LongestCommonSubsequence.GetDistance("hello", "world");

    [Benchmark]
    public void MetricLCSStatic() => _ = F23.StringSimilarity.MetricLCS.GetDistance("hello", "world");

    [Benchmark]
    public void NGramStatic() => _ = F23.StringSimilarity.NGram.GetDistance("hello", "world");

    [Benchmark]
    public void NormalizedLevenshteinStatic() => _ = F23.StringSimilarity.NormalizedLevenshtein.GetDistance("hello", "world");

    [Benchmark]
    public void OptimalStringAlignmentStatic() => _ = F23.StringSimilarity.OptimalStringAlignment.GetDistance("hello", "world");

    [Benchmark]
    public void QGramStatic() => _ = F23.StringSimilarity.QGram.GetDistance("hello", "world");

    [Benchmark]
    public void RatcliffObershelpStatic() => _ = F23.StringSimilarity.RatcliffObershelp.GetDistance("hello", "world");

    [Benchmark]
    public void Sift4Static() => _ = F23.StringSimilarity.Experimental.Sift4.GetDistance("hello", "world");

    [Benchmark]
    public void SorensenDiceStatic() => _ = F23.StringSimilarity.SorensenDice.GetDistance("hello", "world");

    [Benchmark]
    public void WeightedLevenshteinStatic() => _ = F23.StringSimilarity.WeightedLevenshtein.GetDistance("hello", "world", new ExampleCharSub());
#endif

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
}
