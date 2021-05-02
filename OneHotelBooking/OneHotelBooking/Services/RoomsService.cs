using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OneHotelBooking.DbModels;
using OneHotelBooking.Models;

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
                throw new Exception("Not Found"); //TODO:NotFoundException
            }

            return ToRoomInfoModel(dbRoom);
        }

        public async Task<RoomInfo> Add(Room room)
        {
            var isNameNotUnique = await _repository.Get<DbRoom>().AnyAsync(r => r.Number == room.Number);
            if (isNameNotUnique)
            {
                throw new Exception("Name is not unique"); //TODO:ValidationException
            }

            var dbRoom = new DbRoom
            {
                Number = room.Number,
                Description = room.Description,
                Price = room.Price
            };

            _repository.Add(dbRoom);
            await _repository.SaveChangesAsync();

            return ToRoomInfoModel(dbRoom);
        }

        public async Task<RoomInfo> Update(int roomId, Room room)
        {
            var dbRoom = await _repository.Get<DbRoom>().FirstOrDefaultAsync(r => r.Id == roomId);
            if (dbRoom == null)
            {

                throw new Exception("Not Found"); //TODO:NotFoundException
            }

            var isNameNotUnique = await _repository.Get<DbRoom>().AnyAsync(r => r.Number == room.Number && r.Id != roomId);
            if (isNameNotUnique)
            {
                throw new Exception("Name is not unique"); //TODO:ValidationException
            }

            dbRoom.Number = room.Number;
            dbRoom.Description = room.Description;
            dbRoom.Price = room.Price;

            await _repository.SaveChangesAsync();

            return ToRoomInfoModel(dbRoom);
        }

        public async Task Delete(int roomId)
        {
            var dbRoom = await _repository.Get<DbRoom>().FirstOrDefaultAsync(r => r.Id == roomId);
            if (dbRoom == null)
            {
                throw new Exception("Not Found"); //TODO:NotFoundException
            }

            _repository.Remove(dbRoom);
            await _repository.SaveChangesAsync();
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
