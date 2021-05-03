using System;
using OneHotelBooking.DbModels;

namespace OneHotelBooking
{
    public static class DataSeed
    {
        public static readonly DbRoom[] Rooms = new[]
        {
            new DbRoom { Id = 1, Number = 1, Price = 10.55f, Description = "Spacious mountain view room"},
            new DbRoom { Id = 2, Number = 2, Price = 123.45f, Description = "Budget room"}
        };

        public static readonly DbReservation[] Reservations = new[]
        {
            new DbReservation { Id = 1, RoomId = 1, GuestInfo = "John Doe, with a dog", StartDate = new DateTime(2021, 5, 10), EndDate = new DateTime(2021, 5, 13)},
            new DbReservation { Id = 2, RoomId = 1, GuestInfo = "Kate Spring", StartDate = new DateTime(2021, 5, 18), EndDate = new DateTime(2021, 5, 20)},
            new DbReservation { Id = 3, RoomId = 2, GuestInfo = "Ivan Ivanov", StartDate = new DateTime(2021, 5, 5), EndDate = new DateTime(2021, 5, 8)},
        };
    }
}
