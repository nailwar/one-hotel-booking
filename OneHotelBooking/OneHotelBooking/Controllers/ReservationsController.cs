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
    public class ReservationsController : HotelControllerBase
    {
        private readonly ILogger<ReservationsController> _logger;
        private readonly IReservationsService _reservationsService;

        /// <summary>
        /// Controller for Reservations CRUD operations.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="reservationsService">ReservationsService</param>
        public ReservationsController(ILogger<ReservationsController> logger, IReservationsService reservationsService) : base(logger)
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
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public Task<IActionResult> Get([FromQuery]int? roomId = null)
        {
            return roomId.HasValue ?
                ExecuteAsync(async () => new OkObjectResult(await _reservationsService.GetByRoomId(roomId.Value))) :
                ExecuteAsync(async () => new OkObjectResult(await _reservationsService.GetAll()));
        }

        /// <summary>
        /// Gets the reservation by identifier.
        /// </summary>
        /// <param name="id">Reservation identifier.</param>
        /// <returns>ReservationInfo</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReservationInfo), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public Task<IActionResult> Get(int id)
        {
            return ExecuteAsync(async () => new OkObjectResult(await _reservationsService.GetById(id)));
        }

        /// <summary>
        /// Adds new reservation.
        /// </summary>
        /// <param name="reservation">Reservation model.</param>
        /// <returns>ReservationInfo</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ReservationInfo), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public Task<IActionResult> Post([FromBody] Reservation reservation)
        {
            return ExecuteAsync(async () => new OkObjectResult(await _reservationsService.Add(reservation)));
        }

        /// <summary>
        /// Updates the reservation.
        /// </summary>
        /// <param name="id">Reservation identifier.</param>
        /// <param name="reservation">Reservation model.</param>
        /// <returns>ReservationInfo</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ReservationInfo), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public Task<IActionResult> Put(int id, [FromBody] Reservation reservation)
        {
            return ExecuteAsync(async () => new OkObjectResult(await _reservationsService.Update(id, reservation)));
        }

        /// <summary>
        /// Deletes the reservation.
        /// </summary>
        /// <param name="id">Reservation identifier.</param>
        /// <returns>HTTPStatusCode.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public Task<IActionResult> Delete(int id)
        {
            return ExecuteAsync(async () =>
            {
                await _reservationsService.Delete(id);
                return new OkResult();
            });
        }
    }
}
