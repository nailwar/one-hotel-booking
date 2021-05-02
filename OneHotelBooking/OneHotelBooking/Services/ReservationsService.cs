using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OneHotelBooking.DbModels;
using OneHotelBooking.Exceptions;
using OneHotelBooking.Models;
using OneHotelBooking.Repositories;

namespace OneHotelBooking.Services
{
    public class ReservationsService : IReservationsService
    {
        private const int MaxReserveDurationDays = 3;
        private const int MaxReserveAdvanceDays = 30;
        private const int MinReserveAdvanceDays = 1;

        private readonly IRepository _repository;

        public ReservationsService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ReservationInfo[]> GetAll(DateTime startDate = default, DateTime endDate = default)
        {
            if (startDate != default && endDate != default)
            {
                return await _repository.Get<DbReservation>()
                    .Where(r => r.StartDate >= startDate && r.EndDate <= endDate)
                    .Select(r => ToReservationInfoModel(r))
                    .ToArrayAsync();
            }

            return await _repository.Get<DbReservation>()
                .Select(r => ToReservationInfoModel(r))
                .ToArrayAsync();
        }

        public async Task<ReservationInfo> GetById(int reservationId)
        {
            var dbReservation = await _repository.Get<DbReservation>().FirstOrDefaultAsync(r => r.Id == reservationId);
            if (dbReservation == null)
            {
                throw new EntityNotFoundException($"Reservation {reservationId} not found.");
            }

            return ToReservationInfoModel(dbReservation);
        }

        public async Task<ReservationInfo[]> GetByRoomId(int roomId, DateTime startDate = default, DateTime endDate = default)
        {
            var dbRoom = await _repository.Get<DbRoom>().FirstOrDefaultAsync(r => r.Id == roomId);
            if (dbRoom == null)
            {
                throw new EntityNotFoundException($"Room {roomId} not found.");
            }

            if (startDate != default && endDate != default)
            {
                return await _repository.Get<DbReservation>()
                    .Where(r => r.RoomId == roomId && r.StartDate >= startDate && r.EndDate <= endDate)
                    .Select(r => ToReservationInfoModel(r))
                    .ToArrayAsync();
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
                throw new EntityNotFoundException($"Room {reservation.RoomId} not found.");
            }

            NormalizeDates(reservation);

            ValidateDates(reservation.StartDate, reservation.EndDate);

            await CheckDatesForOverlapping(reservation);

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

                throw new EntityNotFoundException($"Reservation {reservationId} not found.");
            }

            var dbRoom = await _repository.Get<DbRoom>().FirstOrDefaultAsync(r => r.Id == reservation.RoomId);
            if (dbRoom == null)
            {
                throw new EntityNotFoundException($"Room {reservation.RoomId} not found.");
            }

            NormalizeDates(reservation);

            ValidateDates(reservation.StartDate, reservation.EndDate);

            await CheckDatesForOverlapping(reservation);

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
                throw new EntityNotFoundException($"Reservation {reservationId} not found.");
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

        private static void NormalizeDates(Reservation reservation)
        {
            reservation.StartDate = reservation.StartDate.StartOfDay();
            reservation.EndDate = reservation.EndDate.StartOfDay();
        }

        private static void ValidateDates(DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
            {
                throw new InputValidationException($"Start date {startDate} can't be after or on the same End date {endDate}");
            }

            var reserveDuration = (int)(endDate - startDate).TotalDays;
            if (reserveDuration > MaxReserveDurationDays)
            {
                throw new InputValidationException($"Reserve duration is too long {reserveDuration} days, set it less or equal than {MaxReserveDurationDays} days.");
            }

            var reserveAdvanceDays = (int)(startDate - DateTime.Today.StartOfDay()).TotalDays;
            if (reserveAdvanceDays < MinReserveAdvanceDays)
            {
                throw new InputValidationException($"Start of reservation should be at least the next day");
            }

            if (reserveAdvanceDays > MaxReserveAdvanceDays)
            {
                throw new InputValidationException($"Start of reservation can't be more than {MaxReserveAdvanceDays} days in advance.");
            }
        }

        private async Task CheckDatesForOverlapping(Reservation reservation)
        {
            var overlappedReservation = await _repository.Get<DbReservation>()
                .FirstOrDefaultAsync(r => r.RoomId == reservation.RoomId &&
                                          reservation.StartDate < r.EndDate &&
                                          reservation.EndDate > r.StartDate);

            if (overlappedReservation != null)
            {
                throw new InputValidationException($"Reservation overlaps another one with id: {overlappedReservation.Id}");
            }
        }
    }
}
