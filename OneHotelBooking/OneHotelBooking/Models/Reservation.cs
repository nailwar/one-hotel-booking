using System;
using System.ComponentModel.DataAnnotations;

namespace OneHotelBooking.Models
{
    public class Reservation
    {
        [Required]
        public int RoomId { get; set; }
        [Required]
        [StringLength(1024)]
        public string GuestInfo { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}
