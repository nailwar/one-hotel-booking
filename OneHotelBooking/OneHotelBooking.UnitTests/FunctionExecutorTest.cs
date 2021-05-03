using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using OneHotelBooking.Exceptions;
using OneHotelBooking.Infrastructure;

namespace OneHotelBooking.UnitTests
{
    
    [TestFixture]
    public class FunctionExecutorTest
    {
        private FunctionExecutor _executor;
        private ILogger _logger;

        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For<ILogger>();
            _executor = new FunctionExecutor(_logger);
        }

        [Test]
        public async Task HotelControllerBase_FunctionExecutedSuccessfully_ReturnsFunctionResult()
        {
            var expected = new OkResult();
            IActionResult SyncFunc() => expected;
            Task<IActionResult> AsyncFunc() => Task.FromResult<IActionResult>(expected);
            
            var actualSync = _executor.Execute(SyncFunc);
            var actualAsync = await _executor.ExecuteAsync(AsyncFunc);
            
            Assert.AreEqual(expected, actualSync);
            Assert.AreEqual(expected, actualAsync);
        }

        [Test]
        public async Task HotelControllerBase_FunctionThrowsKnownException_ReturnsErrorResponse()
        {
            IActionResult SyncFunc() => throw new EntityNotFoundException("");
            Task<IActionResult> AsyncFunc() => Task.Run((Func<IActionResult>)(() => throw new EntityNotFoundException("")));

            var actualSync = (ObjectResult)_executor.Execute(SyncFunc);
            var actualAsync = (ObjectResult)await _executor.ExecuteAsync(AsyncFunc);

            Assert.AreEqual((int)HttpStatusCode.NotFound, actualSync.StatusCode);
            Assert.AreEqual((int)HttpStatusCode.NotFound, actualAsync.StatusCode);

            Assert.IsInstanceOf<ErrorResponse>(actualSync.Value);
            Assert.IsInstanceOf<ErrorResponse>(actualAsync.Value);
        }

        [Test]
        public async Task HotelControllerBase_FunctionThrowsUnknownException_ReturnsErrorResponse()
        {
            IActionResult SyncFunc() => throw new Exception();
            Task<IActionResult> AsyncFunc() => Task.Run((Func<IActionResult>)(() => throw new Exception()));

            var actualSync = (ObjectResult)_executor.Execute(SyncFunc);
            var actualAsync = (ObjectResult)await _executor.ExecuteAsync(AsyncFunc);

            Assert.AreEqual((int)HttpStatusCode.InternalServerError, actualSync.StatusCode);
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, actualAsync.StatusCode);

            Assert.IsInstanceOf<ErrorResponse>(actualSync.Value);
            Assert.IsInstanceOf<ErrorResponse>(actualAsync.Value);
        }
    }
}