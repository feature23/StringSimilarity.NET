using BenchmarkDotNet.Attributes;

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
