using Microsoft.EntityFrameworkCore;
using webRead.Models;
using webRead.Models.webRead.Models;

namespace webRead.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<PushNotification> PushNotifications { get; set; }
        public DbSet<Banner> Banners { get; set; }

        // Configure the model to define the primary key
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define the table name
            modelBuilder.Entity<PushNotification>().ToTable("push_notifications");

            // Define the primary key explicitly
            modelBuilder.Entity<PushNotification>().HasKey(p => p.NotificationId);


            // Define the table name for Banners
            modelBuilder.Entity<Banner>().ToTable("Banners");
            // Define the primary key explicitly for Banners
            modelBuilder.Entity<Banner>().HasKey(b => b.BannerId);
        }
    }
}
