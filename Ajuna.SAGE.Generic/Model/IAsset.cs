using Ajuna.SAGE.Game.Model;

namespace Ajuna.SAGE.Game.Model
{
    /// <summary>
    /// Asset interface
    /// </summary>
    public interface IAsset : IEquatable<IAsset>
    {
        /// <summary>
        /// Identifier
        /// </summary>
        ulong Id { get; set; }

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

        /// <summary>
        /// Match type for same type as
        /// </summary>
        byte[] MatchType { get; }

        /// <summary>
        /// Same type as Asset
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        bool SameTypeAs(IAsset asset);
    }
}