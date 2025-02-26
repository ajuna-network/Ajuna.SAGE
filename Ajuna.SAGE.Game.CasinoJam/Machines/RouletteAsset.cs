using Ajuna.SAGE.Game.Model;

namespace Ajuna.SAGE.Game.CasinoJam.Model
{
    public partial class RouletteAsset : MachineAsset
    {
        public RouletteAsset(uint ownerId, uint genesis)
            : base(ownerId, genesis)
        {
            AssetSubType = (AssetSubType)MachineSubType.Roulette;
            SeatLimit = 1;
        }

        public RouletteAsset(IAsset asset)
            : base(asset)
        { }
    }

    public partial class RouletteAsset
    {

        private static bool CheckBetWin(RouletteBetType betType, byte betDetails,  uint spinResult)
        {
            var details = betDetails;
            switch (betType)
            {
                case RouletteBetType.Straight:
                    return spinResult == betDetails;
                case RouletteBetType.RedBlack:
                    if (spinResult == 0) return false;
                    string spinColor = GetColor(spinResult);
                    string betColor = betDetails == 0 ? "Black" : "Red";
                    return spinColor == betColor;
                case RouletteBetType.EvenOdd:
                    if (spinResult == 0) return false;
                    bool isEven = spinResult % 2 == 0;
                    bool betEven = betDetails == 1; // 0 = Odd, 1 = Even
                    return isEven == betEven;
                case RouletteBetType.LowHigh:
                    if (spinResult == 0) return false;
                    bool isHigh = spinResult > 18;
                    bool betHigh = betDetails == 1; // 0 = Low, 1 = High
                    return isHigh == betHigh;
                case RouletteBetType.Dozen:
                    if (spinResult == 0) return false;
                    byte dozen = betDetails; // 0 = 1-12, 1 = 13-24, 2 = 25-36
                    return (dozen == 0 && spinResult <= 12) ||
                           (dozen == 1 && spinResult > 12 && spinResult <= 24) ||
                           (dozen == 2 && spinResult > 24);
                case RouletteBetType.Column:
                    if (spinResult == 0) return false;
                    byte column = betDetails; // 0, 1, or 2
                    return (spinResult % 3) == (column + 1) % 3;
                // Add Split, Street, Corner, Line as needed with appropriate adjacency checks
                default:
                    return false;
            }
        }

        private static string GetColor(uint number)
        {
            if (number == 0) return "Green";
            uint[] redNumbers = [1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36];
            return redNumbers.Contains(number) ? "Red" : "Black";
        }

        private static uint GetPayoutRatio(RouletteBetType betType)
        {
            return betType switch
            {
                RouletteBetType.Straight => 35,
                RouletteBetType.Split => 17,
                RouletteBetType.Street => 11,
                RouletteBetType.Corner => 8,
                RouletteBetType.Line => 5,
                RouletteBetType.Column => 2,
                RouletteBetType.Dozen => 2,
                RouletteBetType.RedBlack => 1,
                RouletteBetType.EvenOdd => 1,
                RouletteBetType.LowHigh => 1,
                _ => 0
            };
        }
    }

    public enum RouletteBetType : byte
    {
        Straight = 0,   // Single number (payout 35:1)
        Split = 1,      // Two adjacent numbers (payout 17:1)
        Street = 2,     // Three numbers in a row (payout 11:1)
        Corner = 3,     // Four numbers in a square (payout 8:1)
        Line = 4,       // Six numbers, two rows (payout 5:1)
        Column = 5,     // 12 numbers in a column (payout 2:1)
        Dozen = 6,      // 1-12, 13-24, or 25-36 (payout 2:1)
        RedBlack = 7,   // Red or Black (payout 1:1)
        EvenOdd = 8,    // Even or Odd (payout 1:1)
        LowHigh = 9     // 1-18 or 19-36 (payout 1:1)
    }
}
