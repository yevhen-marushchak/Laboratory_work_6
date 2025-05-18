using Cafe.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cafe.DAL
{
    public class CafeDbContext : DbContext
    {
        public CafeDbContext(DbContextOptions<CafeDbContext> options) : base(options) { }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationRoom> ReservationRooms { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReservationRoom>()
                .HasOne(rr => rr.Reservation)
                .WithMany(r => r.ReservationRooms)
                .HasForeignKey(rr => rr.ReservationId);

            modelBuilder.Entity<ReservationRoom>()
                .HasOne(rr => rr.Room)
                .WithMany(r => r.ReservationRooms)
                .HasForeignKey(rr => rr.RoomId);

            modelBuilder.Entity<ReservationRoom>()
                .HasOne(rr => rr.Activity)
                .WithMany()
                .HasForeignKey(rr => rr.ActivityId);

            modelBuilder.Entity<Room>()
                .HasMany(r => r.Activities)
                .WithMany(a => a.Rooms);
        }
    }
}