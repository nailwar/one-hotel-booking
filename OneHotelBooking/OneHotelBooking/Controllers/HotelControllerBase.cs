using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OneHotelBooking.Exceptions;

namespace OneHotelBooking.Controllers
{
    /// <summary>
    /// Base class for all controllers.
    /// </summary>
    public abstract class HotelControllerBase : ControllerBase
    {
        private readonly ILogger _logger;

        protected HotelControllerBase(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Wrapper for controller method body execution, used to generalize exception handling logic.
        /// Executes the specified function.
        /// </summary>
        /// <param name="func">Function that represents expected execution.</param>
        /// <returns>ActionResult.</returns>
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

        /// <summary>
        /// Wrapper for asynchronous controller method body execution, used to generalize exception handling logic.
        /// Executes the specified function as an asynchronous operation.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts exception to expected response.
        /// </summary>
        /// <param name="mappedException">The mapped exception.</param>
        /// <returns>IActionResult.</returns>
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

        /// <summary>
        /// Converts unknown exception to expected response.
        /// </summary>
        /// <param name="e">The mapped exception.</param>
        /// <returns>IActionResult.</returns>
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
