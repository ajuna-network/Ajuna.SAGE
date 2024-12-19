

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

    }
}