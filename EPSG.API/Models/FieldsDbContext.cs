using Microsoft.EntityFrameworkCore;

namespace EPSG.API.Models
{
    public class FieldsDbContext : DbContext
    {
        public FieldsDbContext(DbContextOptions<FieldsDbContext> options) : base(options) { }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Locations> Locations { get; set; }
        public DbSet<LocationPoint> LocationPoints { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Field>()
                .HasOne(f => f.Locations)
                .WithOne()
                .HasForeignKey<Locations>(l => l.Id);
            modelBuilder.Entity<Locations>()
                .HasMany(l => l.Polygon)
                .WithOne()
                .HasForeignKey(lp => lp.Id);
        }
    }
}
