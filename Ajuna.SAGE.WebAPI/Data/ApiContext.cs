using Ajuna.SAGE.Model;
using Microsoft.EntityFrameworkCore;

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
            .Property(p => p.BalanceValue)
            .IsRequired();

        modelBuilder.Entity<DbPlayer>()
            .HasMany(p => p.Assets)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        // DbAsset configuration
        modelBuilder.Entity<DbAsset>()
            .HasKey(a => a.Id);

        modelBuilder.Entity<DbAsset>()
            .Property(a => a.Data) 
            .IsRequired();

        modelBuilder.Entity<DbAsset>()
            .Property(a => a.CollectionId)
            .IsRequired();
    }
}