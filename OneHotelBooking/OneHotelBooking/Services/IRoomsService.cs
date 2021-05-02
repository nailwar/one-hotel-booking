using System.Threading.Tasks;
using OneHotelBooking.Models;

namespace OneHotelBooking.Services
{
    public interface IRoomsService
    {
        Task<RoomInfo[]> GetAll();
        Task<RoomInfo> GetById(int roomId);
        Task<RoomInfo> Add(Room room);
        Task<RoomInfo> Update(int roomId, Room room);
        Task Delete(int roomId);
    }
}