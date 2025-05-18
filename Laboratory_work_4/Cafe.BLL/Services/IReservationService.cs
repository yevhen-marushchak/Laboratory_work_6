using Cafe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cafe.BLL.Services
{
    public interface IReservationService
    {
        Task CreateReservationAsync(
            string customerName,
            DateTime date,
            bool isEventPackage,
            string? additionalDetails,
            List<(int RoomId, int ActivityId)> rooms);

        Task<IEnumerable<Reservation>> GetUserReservationsAsync(string customerName);

        Task<List<DateTime>> GetBookedDatesAsync();

        Task DeleteReservationAsync(int reservationId);
    }
}