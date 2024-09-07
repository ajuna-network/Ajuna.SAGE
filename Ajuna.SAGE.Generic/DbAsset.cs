namespace Ajuna.SAGE.Model
{
    public class DbAsset
    {
        public string Id { get; set; }
        public byte CollectionId { get; set; }
        public uint Score { get; set; }
        public uint Genesis { get; set; }
        public string Dna { get; set; }
        public int PlayerId { get; set; }
        public DbPlayer Player { get; set; }
    }
}
