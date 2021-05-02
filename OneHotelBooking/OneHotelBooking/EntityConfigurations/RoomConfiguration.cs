using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OneHotelBooking.DbModels;

namespace OneHotelBooking.EntityConfigurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<DbRoom>
    {
        public void Configure(EntityTypeBuilder<DbRoom> builder)
        {
            builder.ToTable("Room");

            builder.Property(p => p.Id).HasColumnName("RoomID").ValueGeneratedOnAdd();
            builder.Property(p => p.Number).IsRequired();
            builder.Property(p => p.Price).HasColumnType("numeric(6, 2)");
            builder.Property(p => p.Description).HasColumnType("varchar(2048)");

            builder.HasIndex(p => p.Number).IsUnique();
        }
    }
}
