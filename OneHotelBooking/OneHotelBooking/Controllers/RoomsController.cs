using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OneHotelBooking.Exceptions;
using OneHotelBooking.Models;
using OneHotelBooking.Services;

namespace OneHotelBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : HotelControllerBase
    {
        private readonly ILogger<RoomsController> _logger;
        private readonly IRoomsService _roomsService;

        /// <summary>
        /// Controller for Rooms CRUD operations.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="roomsService">RoomsService</param>
        public RoomsController(ILogger<RoomsController> logger, IRoomsService roomsService) : base(logger)
        {
            _logger = logger;
            _roomsService = roomsService;
        }

        /// <summary>
        /// Gets all rooms.
        /// </summary>
        /// <returns>RoomInfo</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RoomInfo>), (int)HttpStatusCode.OK)]
        public Task<IActionResult> Get()
        {
            return ExecuteAsync(async () => new OkObjectResult(await _roomsService.GetAll()));
        }

        /// <summary>
        /// Gets the room by identifier.
        /// </summary>
        /// <param name="id">Room identifier.</param>
        /// <returns>RoomInfo</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RoomInfo), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public Task<IActionResult> Get(int id)
        {
            return ExecuteAsync(async () => new OkObjectResult(await _roomsService.GetById(id)));
        }

        /// <summary>
        /// Adds new room.
        /// </summary>
        /// <param name="room">Room model.</param>
        /// <returns>RoomInfo</returns>
        [HttpPost]
        [ProducesResponseType(typeof(RoomInfo), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public Task<IActionResult> Post([FromBody] Room room)
        {
            return ExecuteAsync(async () => new OkObjectResult(await _roomsService.Add(room)));
        }

        /// <summary>
        /// Updates the room.
        /// </summary>
        /// <param name="id">Room identifier.</param>
        /// <param name="room">Room model.</param>
        /// <returns>RoomInfo</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RoomInfo), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public Task<IActionResult> Put(int id, [FromBody] Room room)
        {
            return ExecuteAsync(async () => new OkObjectResult(await _roomsService.Update(id, room)));
        }

        /// <summary>
        /// Deletes the room.
        /// </summary>
        /// <param name="id">Room identifier.</param>
        /// <returns>HTTPStatusCode.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public Task<IActionResult> Delete(int id)
        {
            return ExecuteAsync(async () =>
            {
                await _roomsService.Delete(id);
                return new OkResult();
            });
        }
    }
}
