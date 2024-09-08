namespace Ajuna.SAGE.Game.HeroJam
{
    public class HeroJamConstant
    {
        public const byte COLLECTION_ID = 1;

        public const byte BLOCKTIME_SEC = 6;

        public static uint Hour => 60 * 60 / BLOCKTIME_SEC;
    }
}