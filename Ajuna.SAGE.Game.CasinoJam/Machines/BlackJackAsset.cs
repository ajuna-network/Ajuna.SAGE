using Ajuna.SAGE.Game.Model;

namespace Ajuna.SAGE.Game.CasinoJam.Model
{
    public class BlackJackAsset : MachineAsset
    {
        public BlackJackAsset(uint ownerId, uint genesis)
            : base(ownerId, genesis)
        {
            AssetSubType = (AssetSubType)MachineSubType.BlackJack;
            SeatLimit = 1; // Single-player by default, can be adjusted
        }

        public BlackJackAsset(IAsset asset)
            : base(asset)
        { }
    }
}
