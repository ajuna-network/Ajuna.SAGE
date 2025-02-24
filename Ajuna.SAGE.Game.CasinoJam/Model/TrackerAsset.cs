using Ajuna.SAGE.Game.Model;

namespace Ajuna.SAGE.Game.CasinoJam.Model
{
    public class TrackerAsset : PlayerAsset
    {
        public TrackerAsset(uint ownerId, uint genesis)
            : base(ownerId, genesis)
        {
            AssetSubType = (AssetSubType)PlayerSubType.Tracker;
        }

        public TrackerAsset(IAsset asset)
            : base(asset)
        { }

        /// <summary>
        /// SetSlot is a 16-bit field that encodes:
        /// Bits 15-12: Slot1 (4 bits)
        /// Bits 11-8:  Slot2 (4 bits)
        /// Bits 7-4:   Slot3 (4 bits)
        /// Bits 3-2:   Bonus1 (2 bits)
        /// Bits 1-0:   Bonus2 (2 bits)
        /// Stored in Data starting at positions 16 till 22.
        public void SetSlot(byte index, (ushort, byte) packed)
        {
            if (index > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            Data.SetValue<ushort>(16 + (index * 3), packed.Item1);
            Data.SetValue<byte>(18 + (index * 3), packed.Item2);
        }

        /// <summary>
        /// GetSlot is a 16-bit field that encodes:
        /// Bits 15-12: Slot1 (4 bits)
        /// Bits 11-8:  Slot2 (4 bits)
        /// Bits 7-4:   Slot3 (4 bits)
        /// Bits 3-2:   Bonus1 (2 bits)
        /// Bits 1-0:   Bonus2 (2 bits)
        /// Stored in Data starting at positions 16 till 22.
        public (ushort, byte) GetSlot(byte index)
        {
            if (index > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            var sl = Data.ReadValue<ushort>(16 + (index * 3));
            var bn = Data.ReadValue<byte>(18 + (index * 3));
            return (sl, bn);
        }

        /// <summary>
        /// LastReward is a 32-bit field that encodes the last reward value.
        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ........ ....XXXX ........ ........
        /// </summary>
        public uint LastReward
        {
            get => Data.ReadValue<uint>(12);
            set => Data.SetValue<uint>(12, value);
        }
    }
}