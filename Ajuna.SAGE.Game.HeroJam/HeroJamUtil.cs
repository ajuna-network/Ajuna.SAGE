using Ajuna.SAGE.Generic;
using Ajuna.SAGE.Generic.Model;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

[assembly: InternalsVisibleTo("Ajuna.SAGE.Game.HeroJam.Test")]

namespace Ajuna.SAGE.Game.HeroJam
{
    public class HeroJamUtil
    {
        public const byte COLLECTION_ID = 1;

        public const byte BLOCKTIME_SEC = 6;

        public static uint Hour => 60 * 60 / BLOCKTIME_SEC;

        /// <summary>
        /// Get the block time from the action time
        /// </summary>
        /// <param name="actionTime"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static uint GetBlockTimeFrom(ActionTime actionTime)
        {
            return actionTime switch
            {
                ActionTime.Short => 1 * Hour,
                ActionTime.Medium => 3 * Hour,
                ActionTime.Long => 6 * Hour,
                _ => throw new NotSupportedException($"Unsupported ActionTime {actionTime}!"),
            };
        }

        /// <summary>
        /// Get the energy value for the state type
        /// </summary>
        /// <param name="stateType"></param>
        /// <param name="stateSubType"></param>
        /// <param name="actionTime"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static int GetRessourceEnergy(StateType stateType, byte stateSubType, byte actionTime)
        {
            switch (stateType)
            {
                case StateType.None:
                    {
                        return 0;
                    }

                case StateType.Work:
                    {
                        var workType = (WorkType)stateSubType;
                        return workType switch
                        {
                            WorkType.Hunt => -(actionTime * 6),
                            _ => throw new NotImplementedException($"Unknow work type {workType} for energy!"),
                        };
                    }

                case StateType.Sleep:
                    {
                        var sleepType = (SleepType)stateSubType;
                        return sleepType switch
                        {
                            SleepType.Normal => -(actionTime * 1),
                            //SleepType.Fatigue => -(actionTime * 2),
                            _ => throw new NotImplementedException($"Unknow sleep type {sleepType} for energy!"),
                        };
                    }

                default:
                    throw new NotImplementedException($"Unknow state type {stateType} for energy!");
            }
        }

        /// <summary>
        /// Get the fatigue value for the state type
        /// </summary>
        /// <param name="stateType"></param>
        /// <param name="stateSubType"></param>
        /// <param name="actionTime"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static int GetRessourceFatigue(StateType stateType, byte stateSubType, byte actionTime)
        {
            switch (stateType)
            {
                case StateType.None:
                    {
                        return 0;
                    }

                case StateType.Work:
                    {
                        return actionTime * 8;
                    }

                case StateType.Sleep:
                    {
                        var sleepType = (SleepType)stateSubType;
                        return sleepType switch
                        {
                            SleepType.Normal => -(actionTime * actionTime * 11),
                            //SleepType.Fatigue => -(actionTime * actionTime * 5),
                            _ => throw new NotImplementedException($"Unknow sleep type {sleepType} for energy!"),
                        };
                    }

                default:
                    throw new NotImplementedException($"Unknow state type {stateType} for fatigue!");
            }
        }

        public static HeroAsset GetAssetStateTransition(HeroAsset hero, byte[] randomHash, out Asset[] assets)
        {
            // Manage Ressources

            int fatigue = GetRessourceFatigue(hero.StateType, hero.StateSubType, hero.StateSubValue);
            int energy = GetRessourceEnergy(hero.StateType, hero.StateSubType, hero.StateSubValue);

            hero.Fatigue = (byte)Math.Clamp(hero.Fatigue + fatigue, 0, 255);
            hero.Energy = (byte)Math.Clamp(hero.Energy + energy, 0, 255);

            // Manage Assets

            assets = GetAssetStateAssets(hero.StateType, hero.StateSubType, hero.StateSubValue, randomHash, hero.Score);

            // Manage Experience

            int score = GetScore(hero.StateType, hero.StateSubType, hero.StateSubValue);
            hero.Score = (uint)Math.Clamp(hero.Score + score, 0, uint.MaxValue);

            return hero;
        }

        internal static int GetScore(StateType stateType, byte stateSubType, byte stateSubValue)
        {
            return 1;
        }

        internal static Asset[] GetAssetStateAssets(StateType stateType, byte stateSubType, byte stateSubValue, byte[] randomHash, uint score)
        {
            Asset[] assets = [];

            switch (stateType)
            {
                case StateType.Work:

                    switch ((WorkType)stateSubType)
                    {
                        case WorkType.Hunt:
                            assets = GetHuntAssets(stateSubValue, randomHash, score);
                            break;

                        case WorkType.None:
                        default:
                            break;
                    }
                    break;

                case StateType.Sleep:
                    break;

                case StateType.None:
                default:
                    break;
            }

            return assets;
        }

        internal static Asset[] GetHuntAssets(byte stateSubValue, byte[] randomHash, uint score)
        {
            ActionTime actionTime = (ActionTime)stateSubValue;
            Asset[] assets = [];

            var random1 = randomHash[(int)StateType.Work];
            var random2 = randomHash[(int)WorkType.Hunt + 16];

            if (actionTime > 0 && random1 < (50 + (((int)actionTime - 1) * 100)))
            {
                assets = GetHuntAssets(random2, score);
            }

            return assets;
        }

        internal static Asset[] GetHuntAssets(byte random, uint score)
        {
            BaseAsset asset = random switch
            {
                < 1 => CreateAnimal((AssetSubType)AnimalSubType.Duck, 1),
                < 5 => CreateAnimal((AssetSubType)AnimalSubType.Snake, 1),
                < 11 => CreateAnimal((AssetSubType)AnimalSubType.Lizard, 1),
                < 17 => CreateAnimal((AssetSubType)AnimalSubType.Squirrel, 1),
                < 34 => CreateAnimal((AssetSubType)AnimalSubType.Hedgehog, 1),
                < 68 => CreateAnimal( (AssetSubType)AnimalSubType.Mouse, 1),
                _ => CreateAnimal((AssetSubType)AnimalSubType.Insects, 1)
            };
            return [asset];
        }

        internal static BaseAsset[] Disassemble(HeroAsset hero, DisassemblableAsset asset)
        {
            var assetType = asset.Result1AssetType;
            var assetSubType = asset.Result1AssetSubType;
            var assetAmount = asset.Result1Amount;

            return assetType switch
            {
                AssetType.Item => (ItemSubType)assetSubType switch
                {
                    ItemSubType.Meat => [hero, CreateItem(assetSubType, assetAmount)],
                    _ => throw new NotImplementedException($"Unknow asset sub type {assetSubType} in disassemble!"),
                },
                _ => throw new NotImplementedException($"Unknow asset type {assetType} in disassemble!"),
            };
        }

        internal static BaseAsset[] Consume(HeroAsset hero, ConsumableAsset asset)
        {
            var effectHeroStat = asset.Effect1HeroStats;
            var effectValue = asset.Effect1Value;

            switch (effectHeroStat)
            {
                case HeroStats.Energy:
                    {
                        hero.Energy = (byte)Math.Clamp(hero.Energy + effectValue, 0, 100);
                        return [hero];
                    }
                case HeroStats.Fatigue:
                    {
                        hero.Fatigue = (byte)Math.Clamp(hero.Fatigue + effectValue, 0, 100);
                        return [hero];
                    }
                default:
                    throw new NotImplementedException($"Unknow hero stats {effectHeroStat} in consume!");
            }
        }

        internal static BaseAsset CreateItem(AssetSubType assetSubType, byte amount)
        {
            var baseItem = new BaseAssetBuilder(null, COLLECTION_ID, AssetType.Item, (AssetSubType)ItemSubType.Meat)
                .Build();

            var assetFlag = new AssetFlags(0);

            switch ((ItemSubType)assetSubType)
            {
                case ItemSubType.Meat:
                    {
                        var item = new ConsumableAsset(baseItem);
                        assetFlag[(byte)UseType.Consume] = true;

                        item.AssetFlags = assetFlag;
                        item.Effect1HeroStats = HeroStats.Energy;
                        item.Effect1Value = (sbyte)7;
                        return item;
                    }
                default:
                    throw new NotImplementedException($"Unknow animal type {assetSubType} for asset creation!");
            }
        }

        internal static BaseAsset CreateAnimal(AssetSubType assetSubType, byte amount)
        {
            var baseItem = new BaseAssetBuilder(null, COLLECTION_ID, AssetType.Animal, assetSubType)
                .Build();

            var assetFlag = new AssetFlags(0);

            switch((AnimalSubType)assetSubType)
            {
                case AnimalSubType.Duck:
                    {
                        var item = new DisassemblableAsset(baseItem);
                        assetFlag[(byte)UseType.Disassemble] = true;

                        item.AssetFlags = assetFlag;
                        item.Result1AssetType = AssetType.Item;
                        item.Result1AssetSubType = (AssetSubType)ItemSubType.Meat;
                        item.Result1Amount = 4;
                        return item;
                    }
                case AnimalSubType.Snake:
                case AnimalSubType.Lizard:
                case AnimalSubType.Squirrel:
                case AnimalSubType.Hedgehog:
                    {
                        var item = new DisassemblableAsset(baseItem);
                        assetFlag[(byte)UseType.Disassemble] = true;

                        item.AssetFlags = assetFlag;
                        item.Result1AssetType = AssetType.Item;
                        item.Result1AssetSubType = (AssetSubType)ItemSubType.Meat;
                        item.Result1Amount = 2;
                        return item;
                    }
                case AnimalSubType.Mouse:
                    {
                        var item = new DisassemblableAsset(baseItem);
                        assetFlag[(byte)UseType.Disassemble] = true;
                        
                        item.AssetFlags = assetFlag;
                        item.Result1AssetType = AssetType.Item;
                        item.Result1AssetSubType = (AssetSubType)ItemSubType.Meat;
                        item.Result1Amount = 1;
                        return item;
                    }
                case AnimalSubType.Insects:
                    {
                        var item = new ConsumableAsset(baseItem);
                        assetFlag[(byte)UseType.Consume] = true;

                        item.AssetFlags = assetFlag;
                        item.Effect1HeroStats = HeroStats.Energy;
                        item.Effect1Value = (sbyte)3;
                        return item;
                    }
                default:
                    throw new NotImplementedException($"Unknow animal type {assetSubType} for asset creation!");
            }

        }
    }
}