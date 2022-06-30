using Microsoft.EntityFrameworkCore;
using Server.DataAccess.Model;

namespace Server.DataAccess
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() { }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Initial Catalog=DBName;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new Configuration.CitiesConfiguration().Configure(modelBuilder.Entity<Cities>());
            new Configuration.RegionsConfiguration().Configure(modelBuilder.Entity<Regions>());
            new Configuration.MobilAkuConfiguration().Configure(modelBuilder.Entity<MobilAku>());
        }

        public DbSet<MobilAku> MobilAku { get; set; }
        public DbSet<Cities> Cities { get; set; }
        public DbSet<Regions> Regions { get; set; }
    }
}