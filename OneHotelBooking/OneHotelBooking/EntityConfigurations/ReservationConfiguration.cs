using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OneHotelBooking.DbModels;

namespace OneHotelBooking.EntityConfigurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<DbReservation>
    {
        public void Configure(EntityTypeBuilder<DbReservation> builder)
        {
            builder.ToTable("Reservation");

            builder.Property(p => p.Id).HasColumnName("ReservationID").ValueGeneratedOnAdd();
            builder.Property(p => p.RoomId).HasColumnName("RoomID").IsRequired();
            builder.Property(p => p.GuestInfo).HasColumnType("varchar(1024)").IsRequired();
            builder.Property(p => p.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("getutcdate()");
            builder.Property(p => p.StartDate).HasColumnType("datetime").IsRequired();
            builder.Property(p => p.EndDate).HasColumnType("datetime").IsRequired();

            builder.HasData(DataSeed.Reservations);
        }
    }
}