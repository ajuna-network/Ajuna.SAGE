using Ajuna.SAGE.Game.Model;

namespace Ajuna.SAGE.Game.Model
{
    /// <summary>
    /// Asset interface
    /// </summary>
    public interface IAsset : IEquatable<IAsset>, ILockable
    {
        /// <summary>
        /// Identifier
        /// </summary>
        uint Id { get; set; }

        /// <summary>
        /// Owner
        /// </summary>
        uint OwnerId { get; set; }

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