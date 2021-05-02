using System;
using System.Net;

namespace OneHotelBooking.Exceptions
{
    public class EntityNotFoundException : Exception, IResponseMappedException
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }

        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;
        public ErrorResponse ToResponse()
        {
            return new ErrorResponse
            {
                Message = Message,
                ErrorCode = ErrorCode.EntityNotFound
            };
        }
    }
}
