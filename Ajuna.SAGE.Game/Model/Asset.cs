namespace Ajuna.SAGE.Generic.Model
{

    /// <summary>
    /// Asset interface
    /// </summary>
    public interface IAsset : IEquatable<IAsset>
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

    /// <summary>
    /// Asset class
    /// </summary>
    public class Asset : IAsset
    {
        public byte[] Id { get; set; }

        public byte CollectionId { get; }

        public bool Unique { get; set; }

        public uint Score { get; set; }

        public uint Genesis { get; set; }

        public byte[] Data { get; set; }

        /// <summary>
        /// Asset constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collectionId"></param>
        /// <param name="score"></param>
        /// <param name="genesis"></param>
        public Asset(byte[] id, byte collectionId, uint score, uint genesis)
            : this(id, collectionId, score, genesis, new byte[Constants.DNA_SIZE])
        { }

        /// <summary>
        /// Asset constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collectionId"></param>
        /// <param name="score"></param>
        /// <param name="genesis"></param>
        /// <param name="hexData"></param>
        public Asset(byte[] id, byte collectionId, uint score, uint genesis, string hexData)
            : this(id, collectionId, score, genesis, Utils.HexToBytes(hexData))
        { }

        /// <summary>
        /// Asset constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collectionId"></param>
        /// <param name="score"></param>
        /// <param name="genesis"></param>
        /// <param name="data"></param>
        public Asset(byte[] id, byte collectionId, uint score, uint genesis, byte[] data)
        {
            Id = id;
            CollectionId = collectionId;
            Score = score;
            Genesis = genesis;
            Data = data;
        }

        /// <summary>
        /// Empty asset
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collectionId"></param>
        /// <returns></returns>
        public static Asset Empty(byte[] id, byte collectionId)
        {
            Asset avatar = new(id, collectionId, 0, 0);
            return avatar;
        }

        /// <inheritdoc/>
        public bool Equals(IAsset? other)
        {
            if (other == null)
            {
                return false;
            }

            return Id.SequenceEqual(other.Id);
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
    }
}