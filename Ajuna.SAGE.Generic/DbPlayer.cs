namespace Ajuna.SAGE.Model
{
    public class DbPlayer
    {
        public uint Id { get; set; }
        public ICollection<DbAsset>? Assets { get; set; }
        public uint BalanceValue { get; set; }
    }
}
