using Xunit;
using NSubstitute;
using Cafe.BLL.Services;
using Cafe.DAL.Repositories;
using Cafe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cafe.BLL.Test
{
    public class ReservationServiceTests
    {
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly ReservationService _reservationService;

        public ReservationServiceTests()
        {
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _reservationService = new ReservationService(_unitOfWorkMock);
        }

        [Fact]
        public async Task CreateReservationAsync_ShouldThrowException_WhenRoomsAreNullOrEmpty()
        {
            var customerName = "Customer1";
            var date = DateTime.Now;

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _reservationService.CreateReservationAsync(customerName, date, false, null, null));
        }

        [Fact]
        public async Task GetUserReservationsAsync_ShouldReturnReservations()
        {
            var customerName = "Customer1";
            var reservations = new List<Reservation>
            {
                new Reservation { CustomerName = customerName }
            };

            _unitOfWorkMock.ReservationRepository.FindAsync(
                Arg.Any<Expression<Func<Reservation, bool>>>(),
                Arg.Any<Func<IQueryable<Reservation>, IQueryable<Reservation>>>()
            ).Returns(reservations);

            var result = await _reservationService.GetUserReservationsAsync(customerName);

            Assert.Single(result);
        }

        [Fact]
        public async Task DeleteReservationAsync_ShouldThrowException_WhenReservationNotFound()
        {
            var reservationId = 1;
            _unitOfWorkMock.ReservationRepository.GetByIdAsync(reservationId).Returns((Reservation)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _reservationService.DeleteReservationAsync(reservationId));
        }
    }
}