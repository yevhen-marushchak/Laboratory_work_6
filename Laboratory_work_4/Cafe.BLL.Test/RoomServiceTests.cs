using Xunit;
using NSubstitute;
using Cafe.BLL.Services;
using Cafe.DAL.Repositories;
using Cafe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cafe.BLL.Test
{
    public class RoomServiceTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly RoomService _roomService;

        public RoomServiceTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _roomService = new RoomService(_unitOfWorkMock);
        }

        [Fact]
        public async Task GetAllRoomsAsync_ShouldReturnAllRooms()
        {
            var rooms = new List<Room> { new Room { Name = "Room1" }, new Room { Name = "Room2" } };
            _unitOfWorkMock.RoomRepository.GetAllAsync().Returns(rooms);

            var result = await _roomService.GetAllRoomsAsync();

            Assert.Equal(rooms.Count, result.Count());
        }

        [Fact]
        public async Task GetAvailableRoomsAsync_ShouldReturnOnlyAvailableRooms()
        {
            var date = DateTime.Now.Date;
            var rooms = new List<Room>
            {
                new Room { Name = "Room1", IsAvailable = true, ReservationRooms = null },
                new Room { Name = "Room2", IsAvailable = false }
            };

            _unitOfWorkMock.RoomRepository.GetAllAsync(null).ReturnsForAnyArgs(rooms);

            var result = await _roomService.GetAvailableRoomsAsync(date);

            Assert.Single(result);
            Assert.Equal("Room1", result.First().Name);
        }
    }
}