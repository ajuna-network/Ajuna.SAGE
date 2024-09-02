namespace Ajuna.SAGE.Model
{
    public class DbPlayer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<DbAsset> Cards { get; set; }
    }
}