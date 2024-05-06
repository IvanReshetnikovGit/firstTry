using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class AppDbContext : DbContext
    {        
        internal DbSet<Mushroom_picker> Mushroom_picker {get; set; }
        internal DbSet<User> Users{ get; set; }
        internal DbSet<Mushroom> Mushrooms{get; set; }
        internal DbSet<Gather> Gathers{get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "server=localhost;user=root;password=Password;database=Mushroom_data";

            var serverVersion = new MySqlServerVersion(new Version(8,0,36));
            optionsBuilder.UseMySql(connectionString,serverVersion);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mushroom_picker>().HasKey(m => new{m.idmushroom_picker});
            modelBuilder.Entity<User>().HasKey(u=> new{u.Id});
            modelBuilder.Entity<Mushroom>().HasKey(mu => new{mu.Idmushroom});
            modelBuilder.Entity<Gather>().HasKey(g => new{g.IdGather});

            // modelBuilder.Entity<Gather>()
            // .HasOne(g => g.Mushroom)
            // .WithMany()
            // .HasForeignKey(g => g.Idmushroom);

            // modelBuilder.Entity<Gather>()
            // .HasOne(g => g.Mushroom_Picker)
            // .WithMany()
            // .HasForeignKey(g => g.idmushroom_picker);
        }
    }
}
