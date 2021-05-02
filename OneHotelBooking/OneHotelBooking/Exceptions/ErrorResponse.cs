using System.ComponentModel.DataAnnotations;

namespace OneHotelBooking.Exceptions
{
    public class ErrorResponse
    {
        [Required]
        public string Message { get; set; }
        [Required]
        public ErrorCode ErrorCode { get; set; }
    }
}