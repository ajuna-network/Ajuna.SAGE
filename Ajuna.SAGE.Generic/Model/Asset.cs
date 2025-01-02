using Ajuna.SAGE.Game.Model;
using Ajuna.SAGE.Model;
using System.Buffers;

namespace Ajuna.SAGE.Generic.Model
{

    /// <summary>
    /// Asset class
    /// </summary>
    public class Asset : IAsset
    {
        public ulong Id { get; set; }

        public byte CollectionId { get; }

        public uint Score { get; set; }

        public uint Genesis { get; set; }

        public byte[]? Data { get; set; }

        public IBalance Balance { get; set; }

        /// <summary>
        /// Asset constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collectionId"></param>
        /// <param name="score"></param>
        /// <param name="genesis"></param>
        /// <param name="data"></param>
        public Asset(ulong id, byte collectionId, uint score, uint genesis, byte[]? data) 
            : this(id, collectionId, score, genesis, data, new Balance()) { }

        public Asset(ulong id, byte collectionId, uint score, uint genesis, byte[]? data, IBalance balance)
        {
            Id = id;
            CollectionId = collectionId;
            Score = score;
            Genesis = genesis;
            Data = data;
            Balance = balance;
        }

        /// <summary>
        /// Empty asset
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collectionId"></param>
        /// <returns></returns>
        public static Asset Empty(ulong id, byte collectionId)
        {
            Asset avatar = new(id, collectionId, 0, 0, null);
            return avatar;
        }

        /// <inheritdoc/>
        public bool Equals(IAsset? other)
        {
            if (other == null)
            {
                return false;
            }

            return Id == other.Id;
        }

        /// <inheritdoc/>
        public virtual byte[] MatchType => Data != null && Data.Length > 3 ? Data.Take(4).ToArray() : [];

        /// <inheritdoc/>
        public bool SameTypeAs(IAsset other)
        {
            if (other == null)
            {
                return false;
            }

            if (MatchType.Length == 0 || other.MatchType.Length == 0)
            {
                return false;
            }

            return MatchType.SequenceEqual(other.MatchType); ;
        }

        /// <summary>
        /// Map to domain
        /// </summary>
        /// <param name="dbAsset"></param>
        /// <returns></returns>
        public static Asset MapToDomain(DbAsset dbAsset) => 
            new(dbAsset.Id, dbAsset.CollectionId, dbAsset.Score, dbAsset.Genesis, dbAsset.Data, new Balance(dbAsset.BalanceValue));
    }
}