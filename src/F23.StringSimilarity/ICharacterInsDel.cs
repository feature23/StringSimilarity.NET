namespace F23.StringSimilarity
{
    /// <summary>
    /// As an adjunct to <see cref="ICharacterSubstitution"/>, this interface
    /// allows you to specify the cost of deletion or insertion of a
    /// character.
    /// </summary>
    public interface ICharacterInsDel
    {
        /// <summary>
        /// Computes the deletion cost.
        /// </summary>
        /// <param name="c">The character being deleted.</param>
        /// <returns>The cost to be allocated to deleting the given character,
        /// in the range [0, 1].</returns>
        double DeletionCost(char c);

        /// <summary>
        /// Computes the insertion cost.
        /// </summary>
        /// <param name="c">The character being inserted.</param>
        /// <returns>The cost to be allocated to inserting the given character,
        /// in the range [0, 1].</returns>
        double InsertionCost(char c);
    }
}
