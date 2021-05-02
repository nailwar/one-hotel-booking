using System;
using System.Net;

namespace OneHotelBooking.Exceptions
{
    public class InputValidationException : Exception, IResponseMappedException
    {
        public InputValidationException(string message) : base(message)
        {
        }
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
        public ErrorResponse ToResponse()
        {
            return new ErrorResponse()
            {
                ErrorCode = ErrorCode.ModelValidationFailed,
                Message = Message
            };
        }
    }
}
