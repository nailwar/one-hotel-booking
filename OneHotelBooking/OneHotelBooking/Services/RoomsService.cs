using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OneHotelBooking.Exceptions;
using OneHotelBooking.Models;
using OneHotelBooking.DbModels;
using OneHotelBooking.Infrastructure;

namespace OneHotelBooking.Services
{
    public class RoomsService : IRoomsService
    {
        private readonly IRepository _repository;

        public RoomsService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<RoomInfo[]> GetAll()
        {
            return await _repository.Get<DbRoom>()
                .Select(r => ToRoomInfoModel(r))
                .ToArrayAsync();
        }

        public async Task<RoomInfo> GetById(int roomId)
        {
            var dbRoom = await _repository.Get<DbRoom>().FirstOrDefaultAsync(r => r.Id == roomId);
            if (dbRoom == null)
            {
                throw new EntityNotFoundException($"Room {roomId} not found.");
            }

            return ToRoomInfoModel(dbRoom);
        }

        public async Task<RoomInfo> Add(Room room)
        {
            ValidateModel(room);

            var isNumberNotUnique = await _repository.Get<DbRoom>().AnyAsync(r => r.Number == room.Number);
            if (isNumberNotUnique)
            {
                throw new InputValidationException($"Room {room.Number} already added.");
            }

            var dbRoom = new DbRoom
            {
                Number = room.Number.Value,
                Description = room.Description,
                Price = room.Price.Value
            };

            _repository.Add(dbRoom);
            await _repository.SaveChangesAsync();

            return ToRoomInfoModel(dbRoom);
        }

        public async Task<RoomInfo> Update(int roomId, Room room)
        {
            ValidateModel(room);

            var dbRoom = await _repository.Get<DbRoom>().FirstOrDefaultAsync(r => r.Id == roomId);
            if (dbRoom == null)
            {

                throw new EntityNotFoundException($"Room {roomId} not found.");
            }

            var isNumberNotUnique = await _repository.Get<DbRoom>().AnyAsync(r => r.Number == room.Number && r.Id != roomId);
            if (isNumberNotUnique)
            {
                throw new InputValidationException($"Room {room.Number} already exists, use another number.");
            }

            dbRoom.Number = room.Number.Value;
            dbRoom.Description = room.Description;
            dbRoom.Price = room.Price.Value;

            await _repository.SaveChangesAsync();

            return ToRoomInfoModel(dbRoom);
        }

        public async Task Delete(int roomId)
        {
            var dbRoom = await _repository.Get<DbRoom>().FirstOrDefaultAsync(r => r.Id == roomId);
            if (dbRoom == null)
            {
                throw new EntityNotFoundException($"Room {roomId} not found.");
            }

            _repository.Remove(dbRoom);
            await _repository.SaveChangesAsync();
        }

        private static void ValidateModel(Room room)
        {
            if (room == null) { throw new InputValidationException($"{nameof(room)} is null."); }
            if (!room.Number.HasValue) { throw new InputValidationException($"{nameof(room.Number)} is null."); }
            if (!room.Price.HasValue) { throw new InputValidationException($"{nameof(room.Price)} is null."); }
        }

        private static RoomInfo ToRoomInfoModel(DbRoom dbRoom)
        {
            return new RoomInfo
            {
                Id = dbRoom.Id,
                Number = dbRoom.Number,
                Description = dbRoom.Description,
                Price = dbRoom.Price
            };
        }
    }
}
