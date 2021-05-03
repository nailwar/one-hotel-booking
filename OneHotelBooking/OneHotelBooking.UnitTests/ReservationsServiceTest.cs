using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using OneHotelBooking.DateTimeHelpers;
using OneHotelBooking.DbModels;
using OneHotelBooking.Exceptions;
using OneHotelBooking.Infrastructure;
using OneHotelBooking.Models;
using OneHotelBooking.Services;

namespace OneHotelBooking.UnitTests
{
    [TestFixture]
    public class ReservationsServiceTest
    {
        private static readonly DbRoom[] _dbRooms = new[]
        {
            new DbRoom { Id = 1, Number = 1, Description = "Test Desc 1", Price = 10.25f },
            new DbRoom { Id = 2, Number = 2, Description = "Test Desc 2", Price = 1.23f }
        };

        private static readonly DbReservation[] _dbReservations = new[]
        {
            new DbReservation { Id = 1, RoomId = 1, GuestInfo = "G1", StartDate = new DateTime(2021, 5, 10), EndDate = new DateTime(2021, 5, 13)},
            new DbReservation { Id = 2, RoomId = 1, GuestInfo = "G2", StartDate = new DateTime(2021, 5, 18), EndDate = new DateTime(2021, 5, 19)},
            new DbReservation { Id = 3, RoomId = 2, GuestInfo = "G3", StartDate = new DateTime(2021, 5, 5), EndDate = new DateTime(2021, 5, 8)}
        };

        private static readonly ReservationInfo[] _reservationInfos = _dbReservations
            .Select(r => new ReservationInfo { Id = r.Id, RoomId = r.RoomId, GuestInfo = r.GuestInfo, StartDate = r.StartDate, EndDate = r.EndDate })
            .ToArray();

        private ReservationsService _reservationsService;
        private HotelContext _hotelContext;
        private IDateTimeProvider _dateTimeProvider;

        [SetUp]
        public async Task Setup()
        {
            _hotelContext = new HotelContext(new DbContextOptionsBuilder<HotelContext>()
                .UseInMemoryDatabase("hotel_db")
                .Options);

            await _hotelContext.AddRangeAsync(_dbRooms);
            await _hotelContext.AddRangeAsync(_dbReservations);

            await _hotelContext.SaveChangesAsync();

            _dateTimeProvider = Substitute.For<IDateTimeProvider>();
            _dateTimeProvider.TodayStartOfDayDate.Returns(new DateTime(2021, 5, 3));

            _reservationsService = new ReservationsService(new HotelRepository(_hotelContext), _dateTimeProvider);
        }

        [TearDown]
        public async Task TearDown()
        {
            await _hotelContext.Database.EnsureDeletedAsync();
            await _hotelContext.DisposeAsync();
        }

        [Test, Order(1)]
        public async Task GetAll_Called_ReturnsReservations()
        {
            var result = await _reservationsService.GetAll();

            Assert.AreEqual(_reservationInfos.Length, result.Length);
            result.Should().BeEquivalentTo(_reservationInfos);
        }

        [Test]
        public async Task GetAll_CalledWithStartEndParams_ReturnsReservations()
        {
            var startDate = new DateTime(2021, 5, 4);
            var endDate = new DateTime(2021, 5, 9);

            var result = await _reservationsService.GetAll(startDate, endDate);

            Assert.AreEqual(1, result.Length);
            result.Single().Should().BeEquivalentTo(_reservationInfos[2]);
        }

        [Test]
        public async Task GetById_ReservationExists_ReturnsReservation()
        {
            var result = await _reservationsService.GetById(2);
        
            result.Should().BeEquivalentTo(_reservationInfos[1]);
        }
        
        [Test]
        public void GetById_ReservationDoesNotExist_ThrowsException()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _reservationsService.GetById(5));
        }

        [Test]
        public async Task GetByRoomId_ReservationExists_ReturnsReservation()
        {
            var result = await _reservationsService.GetByRoomId(2);

            result.Should().BeEquivalentTo(_reservationInfos[2]);
        }

        [Test]
        public async Task GetByRoomId_CalledWithStartEndParamsReservationExists_ReturnsReservation()
        {
            var startDate = new DateTime(2021, 5, 18);
            var endDate = new DateTime(2021, 5, 20);

            var result = await _reservationsService.GetByRoomId(1, startDate, endDate);

            result.Should().BeEquivalentTo(_reservationInfos[1]);
        }

        [Test]
        public void GetByRoomId_RoomDoesNotExist_ThrowsException()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _reservationsService.GetByRoomId(4));
        }

        [Test]
        public async Task Add_ModelValid_ReturnsNewReservation()
        {
            var toAdd = new Reservation
            {
                RoomId = 1,
                GuestInfo = "A",
                StartDate = new DateTime(2021, 5, 20),
                EndDate = new DateTime(2021, 5, 21)
            };
        
            var result = await _reservationsService.Add(toAdd);
        
            result.Should().BeEquivalentTo(new ReservationInfo
            {
                Id = 4,
                RoomId = toAdd.RoomId.Value,
                GuestInfo = toAdd.GuestInfo,
                StartDate = toAdd.StartDate.Value,
                EndDate = toAdd.EndDate.Value,
            });
        }
        
        [TestCaseSource(nameof(InvalidReservationData))]
        public void Add_ModelInvalid_ThrowsValidationException(Reservation reservation)
        {
            Assert.ThrowsAsync<InputValidationException>(async () => await _reservationsService.Add(reservation));
        }
        
        [TestCaseSource(nameof(InvalidDates))]
        public void Add_InvalidDates_ThrowsValidationException(DateTime startDate, DateTime endDate)
        {
            var reservation = new Reservation
            {
                RoomId = 1,
                GuestInfo = "G4",
                StartDate = startDate,
                EndDate = endDate
            };
            Assert.ThrowsAsync<InputValidationException>(async () => await _reservationsService.Add(reservation));
        }
        
        [Test]
        public async Task Update_ModelValid_ReturnsEditedReservation()
        {
            var reservationId = 1;
            var toUpdate = new Reservation
            {
                RoomId = 2,
                GuestInfo = "U",
                StartDate = new DateTime(2021, 5, 9),
                EndDate = new DateTime(2021, 5, 10)
            };

            var result = await _reservationsService.Update(reservationId, toUpdate);
        
            result.Should().BeEquivalentTo(new ReservationInfo
            {
                Id = reservationId,
                RoomId = toUpdate.RoomId.Value,
                GuestInfo = toUpdate.GuestInfo,
                StartDate = toUpdate.StartDate.Value,
                EndDate = toUpdate.EndDate.Value,
            });
        }
        
        [TestCaseSource(nameof(InvalidReservationData))]
        public void Update_ModelInvalid_ThrowsValidationException(Reservation reservation)
        {
            Assert.ThrowsAsync<InputValidationException>(async () => await _reservationsService.Update(1, reservation));
        }
        
        [Test]
        public void Update_ReservationDoesNotExist_ThrowsException()
        {
            var toUpdate = new Reservation
            {
                RoomId = 2,
                GuestInfo = "U",
                StartDate = new DateTime(2021, 5, 9),
                EndDate = new DateTime(2021, 5, 10)
            };
        
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _reservationsService.Update(5, toUpdate));
        }
        
        [Test]
        public async Task Delete_ReservationExists_DeletesReservation()
        {
            await _reservationsService.Delete(1);
        }
        
        [Test]
        public void Delete_ReservationDoesNotExist_ThrowsException()
        { 
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _reservationsService.Delete(5));
        }
        
        private static IEnumerable<TestCaseData> InvalidReservationData =>
            new List<TestCaseData>
            {
                new TestCaseData(new Reservation { RoomId = null }).SetName("InvalidRoomId"),
                new TestCaseData(new Reservation { RoomId = 1, GuestInfo = null }).SetName("InvalidNullGuestInfo"),
                new TestCaseData(new Reservation { RoomId = 1, GuestInfo = string.Empty }).SetName("InvalidEmptyGuestInfo"),
                new TestCaseData(new Reservation { RoomId = 1, GuestInfo = "G", StartDate = null }).SetName("InvalidStartDate"),
                new TestCaseData(new Reservation { RoomId = 1, GuestInfo = "G", StartDate = new DateTime(2021, 5, 5), EndDate = null }).SetName("InvalidStartDate"),
            };

        private static IEnumerable<TestCaseData> InvalidDates =>
            new List<TestCaseData>
            {
                new TestCaseData(new DateTime(2021, 5, 4), new DateTime(2021, 5, 4)).SetName("StartDateSameEnd"),
                new TestCaseData(new DateTime(2021, 5, 20), new DateTime(2021, 6, 30)).SetName("TooLongReserveDuration"),
                new TestCaseData(new DateTime(2021, 5, 3), new DateTime(2021, 5, 4)).SetName("StartDateToday"),
                new TestCaseData(new DateTime(2021, 6, 4), new DateTime(2021, 6, 5)).SetName("StartDateTooManyDaysAdvance"),
                new TestCaseData(new DateTime(2021, 5, 9), new DateTime(2021, 5, 11)).SetName("EndDateOverlapsAnotherReservation"),
                new TestCaseData(new DateTime(2021, 5, 12), new DateTime(2021, 5, 14)).SetName("StartDateOverlapsAnotherReservation"),
                new TestCaseData(new DateTime(2021, 5, 17), new DateTime(2021, 5, 20)).SetName("StartDateAndEndDateOverlapsAnotherReservation"),
                new TestCaseData(new DateTime(2021, 5, 11), new DateTime(2021, 5, 12)).SetName("StartDateAndEndDateOverlapsAnotherReservation")
            };
    }
}