using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OneHotelBooking.Migrations
{
    public partial class AddDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Room",
                columns: new[] { "RoomID", "Description", "Number", "Price" },
                values: new object[] { 1, "Spacious mountain view room", 1, 10.55m });

            migrationBuilder.InsertData(
                table: "Room",
                columns: new[] { "RoomID", "Description", "Number", "Price" },
                values: new object[] { 2, "Budget room", 2, 123.45m });

            migrationBuilder.InsertData(
                table: "Reservation",
                columns: new[] { "ReservationID", "EndDate", "GuestInfo", "RoomID", "StartDate" },
                values: new object[] { 1, new DateTime(2021, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "John Doe, with a dog", 1, new DateTime(2021, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Reservation",
                columns: new[] { "ReservationID", "EndDate", "GuestInfo", "RoomID", "StartDate" },
                values: new object[] { 2, new DateTime(2021, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kate Spring", 1, new DateTime(2021, 5, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Reservation",
                columns: new[] { "ReservationID", "EndDate", "GuestInfo", "RoomID", "StartDate" },
                values: new object[] { 3, new DateTime(2021, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ivan Ivanov", 1, new DateTime(2021, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reservation",
                keyColumn: "ReservationID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reservation",
                keyColumn: "ReservationID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Reservation",
                keyColumn: "ReservationID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Room",
                keyColumn: "RoomID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Room",
                keyColumn: "RoomID",
                keyValue: 1);
        }
    }
}
