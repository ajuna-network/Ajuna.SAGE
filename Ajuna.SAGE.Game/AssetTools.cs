using System;
using System.Collections.Generic;
using System.Text;
using Ajuna.SAGE.Game.Model;

namespace Ajuna.SAGE.Game
{
    public class AssetTools
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="petType"></param>
        /// <param name="slotType"></param>
        /// <param name="equippableItemType"></param>
        /// <returns></returns>
        public static List<TEnum> CreatePattern<TEnum>(int baseSeed, int increaseSeed)
        {
            byte[] fixSeed = Utils.HexToBytes("2135AAB76B4482CADFF35BB3BD1C86648697B6F6833B47B939AECE95EDCD0347");

            List<TEnum> allEnum = ((TEnum[])Enum.GetValues(typeof(TEnum))).ToList();

            List<TEnum> pattern = new();
            for (int i = 0; i < 4; i++)
            {
                baseSeed += increaseSeed;
                byte rand1 = fixSeed[baseSeed % 32];

                TEnum? enumType = allEnum[rand1 % allEnum.Count];

                // remove element
                allEnum.Remove(enumType);

                // add element to pattern
                pattern.Add(enumType);
            }

            return pattern;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enums"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static byte EnumsToBits<TEnum>(List<TEnum> enums)
        {
            if (enums.Count != enums.Distinct().Count())
            {
                throw new NotSupportedException("Not supporting duplicate elements to encode.");
            }

            Array values = Enum.GetValues(typeof(TEnum));

            if (values.Length > 8)
            {
                throw new NotSupportedException("Not supporting enums with more then 8 variations.");
            }

            int bits = 0;
            foreach (TEnum item in enums)
            {
                int index = Array.IndexOf(values, item);
                if (index >= 0)
                {
                    bits |= (1 << index);
                }
            }
            return (byte)bits;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="bits"></param>
        /// <returns></returns>
        public static List<TEnum> BitsToEnums<TEnum>(byte bits)
        {
            TEnum[] values = (TEnum[])Enum.GetValues(typeof(TEnum));
            List<TEnum> enums = new();
            for (int i = 0; i < values.Length; i++)
            {
                if ((bits & (1 << i)) != 0)
                {
                    enums.Add(values[i]);
                }
            }
            return enums;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enums"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static byte EnumsOrderToBits<TEnum>(List<TEnum> enums)
        {
            List<TEnum> order = enums.OrderBy(p => p).ToList();
            StringBuilder bitsStr = new(enums.Count * 2);
            foreach (TEnum? item in enums)
            {
                int index = order.BinarySearch(item);
                if (index < 0) // item not found in order
                {
                    throw new ArgumentException("The pattern contains items that are not comparable.");
                }
                bitsStr.Append(Convert.ToString(index, 2).PadLeft(2, '0'));
            }
            return Convert.ToByte(bitsStr.ToString(), 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="bitOrder"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<TEnum> BitsOrderToEnums<TEnum>(byte bitOrder, List<TEnum> list)
        {
            TEnum[] order = list.OrderBy(p => p).ToArray();
            List<TEnum> enums = new();
            string bitOrderStr = Convert.ToString(bitOrder, 2).PadLeft(8, '0');
            for (int i = 0; i < bitOrderStr.Length; i += 2)
            {
                string bitStr = bitOrderStr.Substring(i, 2);
                int position = Convert.ToInt32(bitStr, 2);
                if (order.Length > position)
                {
                    enums.Add(order[position]);
                }
            }
            return enums;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rarityType"></param>
        /// <param name="probability"></param>
        /// <param name="progressBytes"></param>
        /// <returns></returns>
        public static byte[] ProgressBytes(RarityType rarityType, int scaleFactor, int? probability, IEnumerable<byte> progressBytes)
        {
            var  codeStruct = progressBytes.ToArray();

            for (int i = 0; i < progressBytes.Count(); i++)
            {
                byte randomValue = codeStruct.Read(i, ByteType.Full);
                RarityType newRarity = rarityType;
                if (probability != null && randomValue * scaleFactor < probability * byte.MaxValue)
                {
                    newRarity++;
                }

                codeStruct.Set(i, ByteType.High, (byte)newRarity);
                codeStruct.Set(i, ByteType.Low, (byte)(randomValue % Constants.PROG_VAR));
            }

            // last progress never is never upgraded (ex. avoid legendary eggs)
            codeStruct.Set(progressBytes.Count() - 1, ByteType.High, (byte)rarityType);

            return codeStruct;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phaseSize"></param>
        /// <param name="totalPhases"></param>
        /// <param name="blockNumber"></param>
        /// <returns></returns>
        public static uint CurrentPeriod(uint phaseSize, uint totalPhases, uint blockNumber)
        {
            return (blockNumber / phaseSize) % totalPhases;
        }
    }
}
