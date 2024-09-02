namespace Ajuna.SAGE.Generic.Model
{
    public static class ByteArrayExtensions
    {
        public static string ToHexDna(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        public static void Set(this byte[] bytes, int pos, ByteType byteType, byte value)
        {
            if (pos < 0 || pos >= bytes.Length)
            {
                throw new NotSupportedException("Out of bounds position.");
            }

            if (byteType != ByteType.Full && value > 15)
            {
                throw new NotSupportedException("Not matching with ByteType.");
            }

            switch (byteType)
            {
                case ByteType.Full:
                    bytes[pos] = value;
                    break;

                case ByteType.High:
                    bytes[pos] = bytes[pos].SetHighNibble(value);
                    break;

                case ByteType.Low:
                    bytes[pos] = bytes[pos].SetLowNibble(value);
                    break;

                default:
                    throw new NotSupportedException("Unsupported ByteType.");
            }
        }

        public static void Set(this byte[] bytes, int pos, byte[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                bytes.Set(pos + i, ByteType.Full, array[i]);
            }
        }

        public static byte Read(this byte[] bytes, int pos, ByteType byteType)
        {
            if (pos < 0 || pos >= bytes.Length)
            {
                throw new NotSupportedException("Out of bounds position.");
            }

            return byteType switch
            {
                ByteType.Full => bytes[pos],
                ByteType.High => bytes[pos].ReadHighNibble(),
                ByteType.Low => bytes[pos].ReadLowNibble(),
                _ => throw new NotSupportedException("Unsupported ByteType."),
            };
        }

        public static byte[] Read(this byte[] bytes, int pos, int length)
        {
            byte[] result = new byte[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = bytes.Read(pos + i, ByteType.Full);
            }
            return result;
        }

        public static byte Lowest(this byte[] bytes, ByteType byteType)
        {
            byte result = byte.MaxValue;
            for (int i = 0; i < bytes.Length; i++)
            {
                byte value = bytes.Read(i, byteType);
                if (result > value)
                {
                    result = value;
                }
            }
            return result;
        }

        public static List<int> LowestIndexes(this byte[] bytes, ByteType byteType)
        {
            byte lowest = byte.MaxValue;
            List<int> result = new();

            for (int i = 0; i < bytes.Length; i++)
            {
                byte value = bytes.Read(i, byteType);
                if (lowest > value)
                {
                    lowest = value;
                    result = new List<int> { i };
                }
                else if (lowest == value)
                {
                    result.Add(i);
                }
            }

            return result;
        }
    }
}