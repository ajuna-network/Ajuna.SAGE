namespace Ajuna.SAGE.Game.CasinoJam
{
    public class SpinResult
    {
        public SpinResult(byte slot1, byte slot2, byte slot3, byte bonus1, byte bonus2)
        {
            Slot1 = slot1;
            Slot2 = slot2;
            Slot3 = slot3;
            Bonus1 = bonus1;
            Bonus2 = bonus2;
        }

        public byte Slot1 { get; }
        public byte Slot2 { get; }
        public byte Slot3 { get; }
        public byte Bonus1 { get; }
        public byte Bonus2 { get; }

        public uint Reward { get; set; }

        public (ushort,byte) Packed => CasinoJamUtil.PackSlotResult(Slot1, Slot2, Slot3, Bonus1, Bonus2);
    }

    public class FullSpin
    {
        public SpinResult[] SpinResults { get; set; }

        public string JackPotResult { get; set; }
        public uint JackPotReward { get; set; }

        public string SpecialResult { get; set; }
        public uint SpecialReward { get; set; }
    }
}