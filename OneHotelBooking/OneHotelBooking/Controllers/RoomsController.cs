using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OneHotelBooking.Models;
using OneHotelBooking.Services;

namespace OneHotelBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly ILogger<RoomsController> _logger;
        private readonly IRoomsService _roomsService;

        /// <summary>
        /// Controller for Rooms CRUD operations.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="roomsService">RoomsService</param>
        public RoomsController(ILogger<RoomsController> logger, IRoomsService roomsService)
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
        public async Task<IActionResult> Get()
        {
            return new OkObjectResult(await _roomsService.GetAll());
        }

        /// <summary>
        /// Gets the room by identifier.
        /// </summary>
        /// <param name="id">Room identifier.</param>
        /// <returns>RoomInfo</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RoomInfo), (int)HttpStatusCode.OK)] //:TODO ErrorResponse
        public async Task<IActionResult> Get(int id)
        {
            return new OkObjectResult(await _roomsService.GetById(id));
        }

        /// <summary>
        /// Adds new room.
        /// </summary>
        /// <param name="room">Room model.</param>
        /// <returns>RoomInfo</returns>
        [HttpPost]
        [ProducesResponseType(typeof(RoomInfo), (int)HttpStatusCode.Created)] //:TODO ErrorResponse
        public async Task<IActionResult> Post([FromBody] Room room)
        {
            return new OkObjectResult(await _roomsService.Add(room));
        }

        /// <summary>
        /// Updates the room.
        /// </summary>
        /// <param name="id">Room identifier.</param>
        /// <param name="room">Room model.</param>
        /// <returns>RoomInfo</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RoomInfo), (int)HttpStatusCode.OK)] //:TODO ErrorResponse
        public async Task<IActionResult> Put(int id, [FromBody] Room room)
        {
            return new OkObjectResult(await _roomsService.Update(id, room));
        }

        /// <summary>
        /// Deletes the room.
        /// </summary>
        /// <param name="id">Room identifier.</param>
        /// <returns>HTTPStatusCode.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)] //:TODO ErrorResponse
        public async Task<IActionResult> Delete(int id)
        {
            await _roomsService.Delete(id);
            return new OkResult();
        }
    }
}
