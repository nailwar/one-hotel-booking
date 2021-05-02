using System.Net;

namespace OneHotelBooking.Exceptions
{
    public interface IResponseMappedException
    {
        HttpStatusCode StatusCode { get; }
        ErrorResponse ToResponse();
    }
}