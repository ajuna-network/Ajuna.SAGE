

namespace Ajuna.SAGE.Game.HeroJam
{
    public class HeroJamUtil
    {
        public const byte COLLECTION_ID = 1;

        public const byte BLOCKTIME_SEC = 6;

        public static uint Hour => 60 * 60 / BLOCKTIME_SEC;

        public static uint ActionTimeToBlocks(ActionTime actionTime)
        {
            switch(actionTime)
            {
                case ActionTime.Short:
                    return 1 * 600;

                case ActionTime.Medium:
                    return 3 * 600;

                case ActionTime.Long:
                    return 6 * 600;

                default:
                    return 0;
            }
        }

        public static int GetRessourceEnergy(StateType stateType, byte stateSubType, byte actionTime)
        {
            switch (stateType)
            {
                case StateType.Idle:
                    {
                        return -(actionTime * 3);
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
                            SleepType.Fatigue => -(actionTime * 2),
                            _ => throw new NotImplementedException($"Unknow sleep type {sleepType} for energy!"),
                        };
                    }

                default:
                    throw new NotImplementedException($"Unknow state type {stateType} for energy!");
            }
        }

        public static int GetRessourceFatigue(StateType stateType, byte stateSubType, byte actionTime)
        {
            switch (stateType)
            {
                case StateType.Idle:
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
                            SleepType.Fatigue => -(actionTime * actionTime * 5),
                            _ => throw new NotImplementedException($"Unknow sleep type {sleepType} for energy!"),
                        };
                    }

                default:
                    throw new NotImplementedException($"Unknow state type {stateType} for fatigue!");
            }
        }

    }
}