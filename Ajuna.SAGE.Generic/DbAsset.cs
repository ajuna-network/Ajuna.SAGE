using Ajuna.SAGE.Game.Model;

namespace Ajuna.SAGE.Model
{
    public class DbAsset
    {
        public uint Id { get; set; }
        public uint OwnerId { get; set; }
        public byte CollectionId { get; set; }
        public uint Score { get; set; }
        public uint Genesis { get; set; }
        public byte[]? Data { get; set; }

        public static DbAsset MapToDb(IAsset asset) => new()
        {
            //Id = asset.Id,
            CollectionId = asset.CollectionId,
            Score = asset.Score,
            Genesis = asset.Genesis,
            Data = asset.Data,
        };
    }
}