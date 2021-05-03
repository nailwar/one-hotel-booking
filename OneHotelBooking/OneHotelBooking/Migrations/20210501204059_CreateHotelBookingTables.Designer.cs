﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OneHotelBooking;
using OneHotelBooking.Infrastructure;

namespace OneHotelBooking.Migrations
{
    [DbContext(typeof(HotelContext))]
    [Migration("20210501204059_CreateHotelBookingTables")]
    partial class CreateHotelBookingTables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OneHotelBooking.DbModels.DbReservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ReservationID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<string>("GuestInfo")
                        .IsRequired()
                        .HasColumnType("varchar(1024)");

                    b.Property<int>("RoomId")
                        .HasColumnType("int")
                        .HasColumnName("RoomID");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Reservation");
                });

            modelBuilder.Entity("OneHotelBooking.DbModels.DbRoom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("RoomID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("varchar(2048)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric(6,2)");

                    b.HasKey("Id");

                    b.HasIndex("Number")
                        .IsUnique();

                    b.ToTable("Room");
                });

            modelBuilder.Entity("OneHotelBooking.DbModels.DbReservation", b =>
                {
                    b.HasOne("OneHotelBooking.DbModels.DbRoom", "Room")
                        .WithMany("Reservations")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("OneHotelBooking.DbModels.DbRoom", b =>
                {
                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}
