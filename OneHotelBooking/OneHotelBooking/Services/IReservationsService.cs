using System;
using System.Threading.Tasks;
using OneHotelBooking.Models;

namespace OneHotelBooking.Services
{
    public interface IReservationsService
    {
        Task<ReservationInfo[]> GetAll(DateTime startDate, DateTime endDate);
        Task<ReservationInfo> GetById(int reservationId);
        Task<ReservationInfo[]> GetByRoomId(int roomId, DateTime startDate, DateTime endDate);
        Task<ReservationInfo> Add(Reservation reservation);
        Task<ReservationInfo> Update(int reservationId, Reservation reservation);
        Task Delete(int reservationId);
    }
}