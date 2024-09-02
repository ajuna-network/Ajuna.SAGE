namespace Ajuna.SAGE.Model
{
    /// <summary>
    /// Asset interface
    /// </summary>
    public interface IAsset
    {
        /// <summary>
        /// Identifier
        /// </summary>
        byte[] Id { get; set; }

        /// <summary>
        /// Collection identifier
        /// </summary>
        byte CollectionId { get; }

        /// <summary>
        /// Score
        /// </summary>
        uint Score { get; set; }

        /// <summary>
        /// Genesis
        /// </summary>
        uint Genesis { get; set; }

        /// <summary>
        /// Custom Data
        /// </summary>
        byte[] Data { get; set; }
    }
}