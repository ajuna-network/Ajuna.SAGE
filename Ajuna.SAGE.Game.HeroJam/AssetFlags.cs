namespace Ajuna.SAGE.Game.HeroJam
{
    public struct AssetFlags
    {
        private bool[] _flags;

        public AssetFlags(byte value)
        {
            _flags = new bool[8];
            _flags[7] = (value & 0b1000_0000) > 0;
            _flags[6] = (value & 0b0100_0000) > 0;
            _flags[5] = (value & 0b0010_0000) > 0;
            _flags[4] = (value & 0b0001_0000) > 0;
            _flags[3] = (value & 0b0000_1000) > 0;
            _flags[2] = (value & 0b0000_0100) > 0;
            _flags[1] = (value & 0b0000_0010) > 0;
            _flags[0] = (value & 0b0000_0001) > 0;
        }

        public readonly bool this[byte index]
        { 
            get => _flags[index];
            set => _flags[index] = value;
        }

        public byte ToByte()
        {
            byte value = 0;
            for (byte i = 0; i < 8; i++)
            {
                if (this[i]) value |= (byte)(1 << i);
            }
            return value;
        }
    }
}
