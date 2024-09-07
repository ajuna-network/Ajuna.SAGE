using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Ajuna.SAGE.Generic
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
        public static byte[] GenerateRandomId()
        {
            var id = new byte[32];
            RandomNumberGenerator.Fill(id);
            return id;
        }
    }
}