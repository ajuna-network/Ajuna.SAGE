using Ajuna.SAGE.Model;
using Microsoft.EntityFrameworkCore;

namespace Ajuna.SAGE.WebAPI.Data
{
    public class ApiContext : DbContext
    {
        public DbSet<DbConfig> Configs { get; set; }
        public DbSet<DbPlayer> Players { get; set; }
        public DbSet<DbAsset> Assets { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // DbConfig configuration
            modelBuilder.Entity<DbConfig>()
                .HasKey(c => c.Id);

            // DbPlayer configuration
            modelBuilder.Entity<DbPlayer>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<DbPlayer>()
                .HasMany(p => p.Cards)
                .WithOne(a => a.Player)
                .HasForeignKey(a => a.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            // DbAsset configuration
            modelBuilder.Entity<DbAsset>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<DbAsset>()
                .Property(a => a.Dna);

            modelBuilder.Entity<DbAsset>()
                .Ignore(a => a.Player);
        }
    }
}
