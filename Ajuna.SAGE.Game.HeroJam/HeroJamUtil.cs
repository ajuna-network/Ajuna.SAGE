using Ajuna.SAGE.Generic.Model;
using System.Xml.Linq;

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

        public static HeroJamAsset GetAssetStateTransition(HeroJamAsset hero, byte[] randomHash, out Asset[] assets)
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

        private static int GetScore(StateType stateType, byte stateSubType, byte stateSubValue)
        {
            return 1;
        }

        private static Asset[] GetAssetStateAssets(StateType stateType, byte stateSubType, byte stateSubValue, byte[] randomHash, uint score)
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

        private static Asset[] GetHuntAssets(byte stateSubValue, byte[] randomHash, uint score)
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

        private static Asset[] GetHuntAssets(byte random, uint score)
        {
            HeroJamAsset asset = new HeroJamAsset(0, COLLECTION_ID, 0, 0);
            asset.AssetType = AssetType.Animal;
            asset.AssetSubType = random switch
            {
                < 1 => (AssetSubType)AnimalType.Duck,
                < 5 => (AssetSubType)AnimalType.Snake,
                < 11 => (AssetSubType)AnimalType.Lizard,
                < 17 => (AssetSubType)AnimalType.Squirrel,
                < 34 => (AssetSubType)AnimalType.Hedgehog,
                < 68 => (AssetSubType)AnimalType.Mouse,
                _ => (AssetSubType)AnimalType.Insects,
            };
            return [asset];
        }
    }
}