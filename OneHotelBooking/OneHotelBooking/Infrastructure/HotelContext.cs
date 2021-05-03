using Microsoft.EntityFrameworkCore;
using OneHotelBooking.EntityConfigurations;

namespace OneHotelBooking.Infrastructure
{
    public class HotelContext : DbContext
    {
        public HotelContext(DbContextOptions<HotelContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
