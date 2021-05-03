using System;
using System.ComponentModel.DataAnnotations;

namespace OneHotelBooking.Models
{
    public class ReservationInfo
    {
        public int Id { get; set; }

        public int RoomId { get; set; }

        public string GuestInfo { get; set; }

        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}
