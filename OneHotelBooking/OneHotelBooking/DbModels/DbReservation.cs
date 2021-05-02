using System;

namespace OneHotelBooking.DbModels
{
    public class DbReservation
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DbRoom Room { get; set; }
        public string GuestInfo { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
