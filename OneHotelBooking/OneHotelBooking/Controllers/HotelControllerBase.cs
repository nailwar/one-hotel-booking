using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OneHotelBooking.Infrastructure;

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
        /// </summary>
        /// <param name="func">Function that represents expected execution.</param>
        /// <returns>ActionResult.</returns>
        protected IActionResult Execute(Func<IActionResult> func)
        {
            return new FunctionExecutor(_logger).Execute(func);
        }

        /// <summary>
        /// Wrapper for asynchronous controller method body execution, used to generalize exception handling logic.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        protected Task<IActionResult> ExecuteAsync(Func<Task<IActionResult>> func)
        {
            return new FunctionExecutor(_logger).ExecuteAsync(func);
        }
    }
}
