using System;

namespace OneHotelBooking.Models
{
    public class ReservationInfo
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string GuestInfo { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
