using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OneHotelBooking.DbModels;
using OneHotelBooking.Exceptions;
using OneHotelBooking.Infrastructure;
using OneHotelBooking.Models;
using OneHotelBooking.Services;

namespace OneHotelBooking.UnitTests
{
    [TestFixture]
    public class RoomServiceTest
    {
        private static readonly DbRoom[] _dbRooms = new[]
        {
            new DbRoom { Id = 1, Number = 1, Description = "Test Desc 1", Price = 10.25f },
            new DbRoom { Id = 2, Number = 2, Description = "Test Desc 2", Price = 1.23f }
        };

        private static readonly RoomInfo[] _roomInfos = _dbRooms
            .Select(r => new RoomInfo { Id = r.Id, Number = r.Number, Price = r.Price, Description = r.Description })
            .ToArray();

        private RoomsService _roomsService;
        private HotelContext _hotelContext;

        [SetUp]
        public async Task Setup()
        {
            _hotelContext = new HotelContext(new DbContextOptionsBuilder<HotelContext>()
                .UseInMemoryDatabase("hotel_db")
                .Options);

            await _hotelContext.AddRangeAsync(_dbRooms);

            await _hotelContext.SaveChangesAsync();

            _roomsService = new RoomsService(new HotelRepository(_hotelContext));
        }

        [TearDown]
        public async Task TearDown()
        {
            await _hotelContext.Database.EnsureDeletedAsync();
            await _hotelContext.DisposeAsync();
        }

        [Test]
        public async Task GetAll_Called_ReturnsRooms()
        {
            var result = await _roomsService.GetAll();

            Assert.AreEqual(_roomInfos.Length, result.Length);
            result.Should().BeEquivalentTo(_roomInfos);
        }

        [Test]
        public async Task GetById_RoomExists_ReturnsRoom()
        {
            var result = await _roomsService.GetById(2);

            result.Should().BeEquivalentTo(_roomInfos[1]);
        }

        [Test]
        public void GetById_RoomDoesNotExist_ThrowsException()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _roomsService.GetById(4));
        }

        [Test]
        public async Task Add_ModelValid_ReturnsNewRoom()
        {
            var toAdd = new Room
            {
                Number = 3,
                Description = "D",
                Price = 1.23f
            };

            var result = await _roomsService.Add(toAdd);

            result.Should().BeEquivalentTo(new RoomInfo
            {
                Id = 3,
                Number = toAdd.Number.Value,
                Price = toAdd.Price.Value,
                Description = toAdd.Description
            });
        }

        [TestCaseSource(nameof(InvalidRoomData))]
        public void Add_ModelInvalid_ThrowsValidationException(Room room)
        {
            Assert.ThrowsAsync<InputValidationException>(async () => await _roomsService.Add(room));
        }

        [Test]
        public async Task Update_ModelValid_ReturnsEditedRoom()
        {
            var roomId = 2;
            var toUpdate = new Room
            {
                Number = 3,
                Description = "C",
                Price = 10f
            };

            var result = await _roomsService.Update(roomId, toUpdate);

            result.Should().BeEquivalentTo(new RoomInfo
            {
                Id = roomId,
                Number = toUpdate.Number.Value,
                Price = toUpdate.Price.Value,
                Description = toUpdate.Description
            });
        }

        [TestCaseSource(nameof(InvalidRoomData))]
        public void Update_ModelInvalid_ThrowsValidationException(Room room)
        {
            Assert.ThrowsAsync<InputValidationException>(async () => await _roomsService.Update(1, room));
        }

        [Test]
        public void Update_RoomDoesNotExist_ThrowsException()
        {
            var toUpdate = new Room
            {
                Number = 3,
                Price = 1f,
                Description = "D"
            };

            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _roomsService.Update(4, toUpdate));
        }

        [Test]
        public async Task Delete_RoomExists_DeletesRoom()
        {
            await _roomsService.Delete(1);
        }

        [Test]
        public void Delete_RoomDoesNotExist_ThrowsException()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _roomsService.Delete(4));
        }

        private static IEnumerable<TestCaseData> InvalidRoomData =>
            new List<TestCaseData>
            {
                new TestCaseData(new Room { Number = null }).SetName("InvalidNumber"),
                new TestCaseData(new Room { Number = 3, Price = null }).SetName("InvalidPrice"),
                new TestCaseData(new Room { Number = 2, Price = 1f, Description = "D"}).SetName("NumberAlreadyExists")
            };
    }
}