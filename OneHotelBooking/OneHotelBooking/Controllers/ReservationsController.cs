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
    public class ReservationsController : ControllerBase
    {
        private readonly ILogger<ReservationsController> _logger;
        private readonly IReservationsService _reservationsService;

        /// <summary>
        /// Controller for Reservations CRUD operations.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="reservationsService">ReservationsService</param>
        public ReservationsController(ILogger<ReservationsController> logger, IReservationsService reservationsService)
        {
            _logger = logger;
            _reservationsService = reservationsService;
        }

        /// <summary>
        /// Gets all reservations.
        /// </summary>
        /// <returns>ReservationInfo</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ReservationInfo>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery]int? roomId = null)
        {
            return roomId.HasValue ? 
                new OkObjectResult(await _reservationsService.GetByRoomId(roomId.Value)) : 
                new OkObjectResult(await _reservationsService.GetAll());
        }

        /// <summary>
        /// Gets the reservation by identifier.
        /// </summary>
        /// <param name="id">Reservation identifier.</param>
        /// <returns>ReservationInfo</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReservationInfo), (int)HttpStatusCode.OK)] //:TODO ErrorResponse
        public async Task<IActionResult> Get(int id)
        {
            return new OkObjectResult(await _reservationsService.GetById(id));
        }

        /// <summary>
        /// Adds new reservation.
        /// </summary>
        /// <param name="reservation">Reservation model.</param>
        /// <returns>ReservationInfo</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ReservationInfo), (int)HttpStatusCode.Created)] //:TODO ErrorResponse
        public async Task<IActionResult> Post([FromBody] Reservation reservation)
        {
            return new OkObjectResult(await _reservationsService.Add(reservation));
        }

        /// <summary>
        /// Updates the reservation.
        /// </summary>
        /// <param name="id">Reservation identifier.</param>
        /// <param name="reservation">Reservation model.</param>
        /// <returns>ReservationInfo</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ReservationInfo), (int)HttpStatusCode.OK)] //:TODO ErrorResponse
        public async Task<IActionResult> Put(int id, [FromBody] Reservation reservation)
        {
            return new OkObjectResult(await _reservationsService.Update(id, reservation));
        }

        /// <summary>
        /// Deletes the reservation.
        /// </summary>
        /// <param name="id">Reservation identifier.</param>
        /// <returns>HTTPStatusCode.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)] //:TODO ErrorResponse
        public async Task<IActionResult> Delete(int id)
        {
            await _reservationsService.Delete(id);
            return new OkResult();
        }
    }
}
