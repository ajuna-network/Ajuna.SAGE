using Ajuna.SAGE.Game.Model;
using Ajuna.SAGE.Generic.Model;

namespace Ajuna.SAGE.Model
{
    public class DbPlayer
    {
        public ulong Id { get; set; }
        public List<DbAsset> Assets { get; set; }
        public uint BalanceValue { get; set; }
    }
}
