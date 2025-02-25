namespace Ajuna.SAGE.Game.Model
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

        /// <summary>
        /// Writes a value of type T into the byte array starting at the given position.
        /// Supports types: byte, ushort, uint.
        /// </summary>
        public static void SetValue<T>(this byte[] bytes, int pos, T value) where T : struct
        {
            byte[] arr = typeof(T) switch
            {
                Type t when t == typeof(byte) => [(byte)(object)value],
                Type t when t == typeof(ushort) => BitConverter.GetBytes((ushort)(object)value),
                Type t when t == typeof(uint) => BitConverter.GetBytes((uint)(object)value),
                _ => throw new NotSupportedException($"Type {typeof(T).Name} is not supported.")
            };

            if (pos < 0 || pos + arr.Length > bytes.Length)
            {
                throw new NotSupportedException("Out of bounds position.");
            }

            for (int i = 0; i < arr.Length; i++)
            {
                bytes[pos + i] = arr[i];
            }
        }

        /// <summary>
        /// Reads a value of type T from the byte array starting at the given position.
        /// Supports types: byte, ushort, uint.
        /// </summary>
        public static T ReadValue<T>(this byte[] bytes, int pos) where T : struct
        {
            if (pos < 0)
                throw new NotSupportedException("Out of bounds position.");

            if (typeof(T) == typeof(byte))
            {
                return (T)(object)bytes[pos];
            }
            else if (typeof(T) == typeof(ushort))
            {
                return (T)(object)BitConverter.ToUInt16(bytes, pos);
            }
            else if (typeof(T) == typeof(uint))
            {
                return (T)(object)BitConverter.ToUInt32(bytes, pos);
            }
            else
            {
                throw new NotSupportedException($"Type {typeof(T).Name} is not supported.");
            }
        }


    }
}