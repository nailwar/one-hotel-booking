using System.ComponentModel.DataAnnotations;

namespace OneHotelBooking.Models
{
    public class Room
    {
        [Required]
        public int Number { get; set; }
        public float Price { get; set; }
        [StringLength(2048)]
        public string Description { get; set; }
    }
}
