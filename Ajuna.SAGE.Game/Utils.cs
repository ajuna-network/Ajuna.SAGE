using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Ajuna.SAGE.Game
{
    public static class Utils
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static byte[] HexToBytes(string hexString)
        {
            if(hexString.StartsWith("0x"))
            {
                hexString = hexString[2..];
            }

            if (hexString.Length % 2 == 1 || !Regex.IsMatch(hexString, "^[0-9A-Fa-f]*$"))
            {
                throw new NotSupportedException("The binary key cannot have an odd number of digits");
            }

            byte[] arr = new byte[hexString.Length >> 1];

            for (int i = 0; i < hexString.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hexString[i << 1]) << 4) + (GetHexVal(hexString[(i << 1) + 1])));
            }

            return arr;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static int GetHexVal(char hex)
        {
            int val = hex;
            if (val < 97)
            {
                return val - (val < 58 ? 48 : 55);
            }
            else
            {
                return val - (val < 58 ? 48 : 87);
            }
        }

        /// <summary>
        /// Get a Random enumeration.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="random"></param>
        /// <returns></returns>
        public static TEnum RandomEnum<TEnum>(Random random)
        {
            TEnum[] items = (TEnum[])Enum.GetValues(typeof(TEnum));
            return items[random.Next(0, items.Length - 1)];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="main"></param>
        /// <returns></returns>
        public static byte ReadHighNibble(this byte main)
        {
            return (byte)((byte)(main & 0xF0) >> 4);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="main"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static byte SetHighNibble(this byte main, byte value)
        {
            if (value > 15)
            {
                throw new NotSupportedException("Out of bounds.");
            }
            return (byte)(main & 0x0F | (byte)(value << 4));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="main"></param>
        /// <returns></returns>
        public static byte ReadLowNibble(this byte main)
        {
            return (byte)(main & 0x0F);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="main"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static byte SetLowNibble(this byte main, byte value)
        {
            if (value > 15)
            {
                throw new NotSupportedException("Out of bounds.");
            }
            return (byte)(main & 0xF0 | (byte)(value << 4 >> 4));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static List<int> IndexesOfMax(byte[] array)
        {
            byte maxVal = byte.MinValue;  // initial maximum value
            List<int> maxIndexes = new();  // list of indices of maximum values

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] > maxVal)
                {
                    maxVal = array[i];
                    maxIndexes = new List<int>
                    {
                        i  // add this index
                    };
                }
                else if (array[i] == maxVal)
                {
                    maxIndexes.Add(i);  // add this index
                }
            }

            return maxIndexes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        /// <summary>
        /// Generate a random id
        /// </summary>
        /// <returns></returns>
        public static uint GenerateRandomId()
        {
            var id = new byte[4];
            RandomNumberGenerator.Fill(id);
            return BitConverter.ToUInt32(id);
        }

        /// <summary>
        /// Compares the first <paramref name="bitsToCompare"/> bits of two byte arrays.
        /// </summary>
        /// <param name="data1">The first data array.</param>
        /// <param name="data2">The second data array.</param>
        /// <param name="bitsToCompare">The number of starting bits to compare.</param>
        /// <returns>
        /// True if the first <paramref name="bitsToCompare"/> bits in both arrays are equal; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if either array is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if bitsToCompare is less than 1.</exception>
        /// <exception cref="ArgumentException">Thrown if either array is too short to compare the given number of bits.</exception>
        public static bool ComparePrefix(byte[] data1, byte[] data2, int bitsToCompare)
        {
            if (data1 == null || data2 == null)
                throw new ArgumentNullException("Data arrays cannot be null.");

            if (bitsToCompare < 1)
                throw new ArgumentOutOfRangeException(nameof(bitsToCompare), "The number of bits to compare must be at least 1.");

            // Calculate how many bytes are needed to cover the requested bits.
            int neededBytes = (bitsToCompare + 7) / 8;  // Ceiling division.
            if (data1.Length < neededBytes || data2.Length < neededBytes)
                throw new ArgumentException("One or both data arrays are too short to compare the given number of bits.");

            // First, compare the full bytes.
            int fullBytes = bitsToCompare / 8;
            for (int i = 0; i < fullBytes; i++)
            {
                if (data1[i] != data2[i])
                    return false;
            }

            // Compare the remaining bits, if any.
            int remainingBits = bitsToCompare % 8;
            if (remainingBits > 0)
            {
                // Create a mask for the high bits in the next byte.
                // For example, if remainingBits is 3, then the mask will be 0xE0 (11100000 in binary).
                byte mask = (byte)(0xFF << (8 - remainingBits));
                if ((data1[fullBytes] & mask) != (data2[fullBytes] & mask))
                    return false;
            }

            return true;
        }
    }
}