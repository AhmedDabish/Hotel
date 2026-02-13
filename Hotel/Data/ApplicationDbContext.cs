// Data/ApplicationDbContext.cs
using Hotel.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<HotelSetting> HotelSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // تكوين خصائص Decimal لتجنب التحذيرات
            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<HotelSetting>()
                .Property(h => h.DeluxePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<HotelSetting>()
                .Property(h => h.SuitePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<HotelSetting>()
                .Property(h => h.PresidentialPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Room>()
                .Property(r => r.PricePerNight)
                .HasPrecision(18, 2);

            // إعدادات إضافية
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Booking>()
                .HasIndex(b => b.Email);

            modelBuilder.Entity<Booking>()
                .HasIndex(b => b.Status);

            modelBuilder.Entity<Room>()
                .HasIndex(r => r.Type);

            modelBuilder.Entity<Room>()
                .HasIndex(r => r.IsAvailable);
        }
    }
}