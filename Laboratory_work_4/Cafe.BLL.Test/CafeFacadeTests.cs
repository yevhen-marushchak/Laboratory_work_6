using Xunit;
using NSubstitute;
using Cafe.BLL.Facades;
using Cafe.BLL.Services;
using Cafe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cafe.BLL.Test
{
    public class CafeFacadeTests
    {
        private readonly IReservationService _reservationServiceMock;
        private readonly IRoomService _roomServiceMock;
        private readonly IUserService _userServiceMock;
        private readonly CafeFacade _cafeFacade;

        public CafeFacadeTests()
        {
            _reservationServiceMock = Substitute.For<IReservationService>();
            _roomServiceMock = Substitute.For<IRoomService>();
            _userServiceMock = Substitute.For<IUserService>();

            _cafeFacade = new CafeFacade(_reservationServiceMock, _roomServiceMock, _userServiceMock);
        }

        [Fact]
        public async Task GetAllRoomsAsync_ShouldReturnRooms()
        {
            var rooms = new List<Room> { new Room { Name = "Room1" } };
            _roomServiceMock.GetAllRoomsAsync().Returns(rooms);

            var result = await _cafeFacade.GetAllRoomsAsync();

            Assert.Single(result);
            Assert.Equal("Room1", result.First().Name);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnUser_WhenCredentialsAreValid()
        {
            var username = "testuser";
            var password = "password";
            var user = new User { Username = username };
            _userServiceMock.LoginAsync(username, password).Returns(user);

            var result = await _cafeFacade.LoginAsync(username, password);

            Assert.NotNull(result);
            Assert.Equal(username, result.Username);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnTrue_WhenUserIsNew()
        {
            var username = "newuser";
            var password = "password";
            _userServiceMock.RegisterAsync(username, password).Returns(true);

            var result = await _cafeFacade.RegisterAsync(username, password);

            Assert.True(result);
        }

        [Fact]
        public async Task CreateReservationAsync_ShouldCallReservationService()
        {
            var customerName = "Customer1";
            var date = DateTime.Now;
            var isEventPackage = true;
            var additionalDetails = "Details";
            var rooms = new List<(int RoomId, int ActivityId)>
            {
                (1, 1),
                (2, 2)
            };

            await _cafeFacade.CreateReservationAsync(customerName, date, isEventPackage, additionalDetails, rooms);

            await _reservationServiceMock.Received(1).CreateReservationAsync(customerName, date, isEventPackage, additionalDetails, rooms);
        }

        [Fact]
        public async Task GetUserReservationsAsync_ShouldReturnReservations()
        {
            var customerName = "Customer1";
            var reservations = new List<Reservation>
            {
                new Reservation { CustomerName = customerName }
            };
            _reservationServiceMock.GetUserReservationsAsync(customerName).Returns(reservations);

            var result = await _cafeFacade.GetUserReservationsAsync(customerName);

            Assert.Single(result);
            Assert.Equal(customerName, result.First().CustomerName);
        }

        [Fact]
        public async Task GetBookedDatesAsync_ShouldReturnDates()
        {
            var bookedDates = new List<DateTime> { DateTime.Now.Date };
            _reservationServiceMock.GetBookedDatesAsync().Returns(bookedDates);

            var result = await _cafeFacade.GetBookedDatesAsync();

            Assert.Single(result);
            Assert.Equal(bookedDates.First(), result.First());
        }

        [Fact]
        public async Task DeleteReservationAsync_ShouldCallReservationService()
        {
            var reservationId = 1;

            await _cafeFacade.DeleteReservationAsync(reservationId);

            await _reservationServiceMock.Received(1).DeleteReservationAsync(reservationId);
        }
    }
}