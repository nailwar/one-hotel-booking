using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OneHotelBooking.Exceptions;

namespace OneHotelBooking.Controllers
{
    public abstract class HotelControllerBase : ControllerBase
    {
        private readonly ILogger _logger;

        protected HotelControllerBase(ILogger logger)
        {
            _logger = logger;
        }

        protected IActionResult Execute(Func<IActionResult> func)
        {
            try
            {
                return func();
            }
            catch (Exception e) when (e is IResponseMappedException mappedException)
            {
                return ToExpectedResponse(mappedException);
            }
            catch (Exception e)
            {
                return ToUnknownResponse(e);
            }
        }

        protected async Task<IActionResult> ExecuteAsync(Func<Task<IActionResult>> func)
        {
            try
            {
                return await func();
            }
            catch (Exception e) when (e is IResponseMappedException mappedException)
            {
                return ToExpectedResponse(mappedException);
            }
            catch (Exception e)
            {
                return ToUnknownResponse(e);
            }
        }

        private IActionResult ToExpectedResponse(IResponseMappedException mappedException)
        {
            var errorData = mappedException.ToResponse();
            if (errorData.ErrorCode != ErrorCode.ModelValidationFailed)
            {
                _logger.LogError((Exception)mappedException, errorData.Message);
            }

            return new ObjectResult(errorData)
            {
                StatusCode = (int) mappedException.StatusCode
            };
        }

        private IActionResult ToUnknownResponse(Exception e)
        {
            _logger.LogError(e, "Unknown error while executing request");

            return new ObjectResult(
                new ErrorResponse
                {
                    Message = "Unknown error has occurred.",
                    ErrorCode = ErrorCode.Unknown,
                })
            {
                StatusCode = 500,
            };
        }
    }
}
