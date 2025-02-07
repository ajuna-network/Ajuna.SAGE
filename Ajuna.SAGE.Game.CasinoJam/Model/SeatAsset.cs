using Ajuna.SAGE.Game.Model;

namespace Ajuna.SAGE.Game.CasinoJam.Model
{
    public class SeatAsset : BaseAsset
    {
        public SeatAsset(uint genesis)
            : base(0, genesis)
        {
            AssetType = AssetType.Seat;
        }

        public SeatAsset(IAsset asset)
            : base(asset)
        { }

    }
}