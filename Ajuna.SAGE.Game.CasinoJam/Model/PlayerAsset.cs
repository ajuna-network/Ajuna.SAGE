using Ajuna.SAGE.Game.Model;
using System.Security.Cryptography;

namespace Ajuna.SAGE.Game.CasinoJam.Model
{
    /// <summary>
    ///
    /// </summary>
    public class PlayerAsset : BaseAsset
    {
        public PlayerAsset(uint ownerId, uint genesis)
            : base(ownerId, 0, genesis)
        {
            AssetType = AssetType.Player;
        }

        public PlayerAsset(IAsset asset)
            : base(asset)
        { }
    }

    public class HumanAsset : PlayerAsset
    {
        public HumanAsset(uint ownerId, uint genesis)
            : base(ownerId, genesis)
        {
            AssetSubType = (AssetSubType)PlayerSubType.Human;
        }

        public HumanAsset(IAsset asset)
            : base(asset)
        { }

        /// <summary>
        /// The identifier of the seat associated with this player.
        /// Stored as 4 bytes at offset 28.
        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ........ ........ ........ ....XXXX
        /// </summary>
        public uint SeatId
        {
            get => Data.ReadValue<uint>(28);
            set => Data.SetValue<uint>(28, value);
        }

        /// <summary>
        /// Release the seat.
        /// </summary>
        public void Release()
        {
            SeatId = 0;
        }
    }
}