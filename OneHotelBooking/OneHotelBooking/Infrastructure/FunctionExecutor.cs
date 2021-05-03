using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OneHotelBooking.Exceptions;

namespace OneHotelBooking.Infrastructure
{
    public class FunctionExecutor
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionExecutor"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public FunctionExecutor(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Executes the specified function.
        /// </summary>
        /// <param name="func">Function that represents expected execution.</param>
        /// <returns>ActionResult.</returns>
        public IActionResult Execute(Func<IActionResult> func)
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
        /// Executes the specified function as an asynchronous operation.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<IActionResult> ExecuteAsync(Func<Task<IActionResult>> func)
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
                StatusCode = (int)mappedException.StatusCode
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
