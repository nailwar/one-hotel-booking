using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OneHotelBooking.DbModels;
using OneHotelBooking.Models;

namespace OneHotelBooking.Services
{
    public class ReservationsService : IReservationsService
    {
        private readonly IRepository _repository;

        public ReservationsService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ReservationInfo[]> GetAll() //TODO:Add From and To
        {
            return await _repository.Get<DbReservation>()
                .Select(r => ToReservationInfoModel(r))
                .ToArrayAsync();
        }

        public async Task<ReservationInfo> GetById(int reservationId)
        {
            var dbReservation = await _repository.Get<DbReservation>().FirstOrDefaultAsync(r => r.Id == reservationId);
            if (dbReservation == null)
            {
                throw new Exception("Not Found"); //TODO:NotFoundException
            }

            return ToReservationInfoModel(dbReservation);
        }

        public async Task<ReservationInfo[]> GetByRoomId(int roomId) //TODO:Add From and To
        {
            var dbRoom = await _repository.Get<DbRoom>().FirstOrDefaultAsync(r => r.Id == roomId);
            if (dbRoom == null)
            {
                throw new Exception("Room Not Found"); //TODO:NotFoundException
            }

            return await _repository.Get<DbReservation>()
                .Where(r => r.RoomId == roomId)
                .Select(r => ToReservationInfoModel(r))
                .ToArrayAsync();
        }

        public async Task<ReservationInfo> Add(Reservation reservation)
        {
            var dbRoom = await _repository.Get<DbRoom>().FirstOrDefaultAsync(r => r.Id == reservation.RoomId);
            if (dbRoom == null)
            {
                throw new Exception("Room Not Found"); //TODO:NotFoundException
            }

            //TODO Check for add ability

            var dbReservation = new DbReservation
            {
                RoomId = reservation.RoomId,
                GuestInfo = reservation.GuestInfo,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate
            };

            _repository.Add(dbReservation);
            await _repository.SaveChangesAsync();

            return ToReservationInfoModel(dbReservation);
        }

        public async Task<ReservationInfo> Update(int reservationId, Reservation reservation)
        {
            var dbReservation = await _repository.Get<DbReservation>().FirstOrDefaultAsync(r => r.Id == reservationId);
            if (dbReservation == null)
            {

                throw new Exception("Not Found"); //TODO:NotFoundException
            }

            var dbRoom = await _repository.Get<DbRoom>().FirstOrDefaultAsync(r => r.Id == reservation.RoomId);
            if (dbRoom == null)
            {
                throw new Exception("Room Not Found"); //TODO:NotFoundException
            }

            //TODO Check for update ability

            dbReservation.RoomId = reservation.RoomId;
            dbReservation.GuestInfo = reservation.GuestInfo;
            dbReservation.StartDate = reservation.StartDate;
            dbReservation.EndDate = reservation.EndDate;

            await _repository.SaveChangesAsync();

            return ToReservationInfoModel(dbReservation);
        }

        public async Task Delete(int reservationId)
        {
            var dbReservation = await _repository.Get<DbReservation>().FirstOrDefaultAsync(r => r.Id == reservationId);
            if (dbReservation == null)
            {
                throw new Exception("Not Found"); //TODO:NotFoundException
            }

            _repository.Remove(dbReservation);
            await _repository.SaveChangesAsync();
        }

        private static ReservationInfo ToReservationInfoModel(DbReservation dbReservation)
        {
            return new ReservationInfo
            {
                Id = dbReservation.Id,
                RoomId = dbReservation.RoomId,
                GuestInfo = dbReservation.GuestInfo,
                CreatedDate = dbReservation.CreatedDate,
                StartDate = dbReservation.StartDate,
                EndDate = dbReservation.EndDate
            };
        }
    }
}
