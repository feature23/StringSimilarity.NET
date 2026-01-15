namespace F23.StringSimilarity.Interfaces
{
    /// <summary>
    /// Defines a contract for calculating the similarity between spans of text, normalized to a range of 0 to 1.
    /// </summary>
    /// <remarks>This interface extends <see cref="ISpanSimilarity"/> by ensuring that similarity scores are
    /// normalized. A score of 0 indicates no similarity, while a score of 1 indicates identical spans.</remarks>
    public interface INormalizedSpanSimilarity : ISpanSimilarity
    {
    }
}