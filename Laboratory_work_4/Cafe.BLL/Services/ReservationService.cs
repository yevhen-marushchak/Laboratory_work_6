using Cafe.DAL.Repositories;
using Cafe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Cafe.BLL.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReservationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateReservationAsync(
            string customerName,
            DateTime date,
            bool isEventPackage,
            string? additionalDetails,
            List<(int RoomId, int ActivityId)> rooms)
        {
            if (rooms == null || rooms.Count < 1)
                throw new InvalidOperationException("Потрібно обрати хоча б одну залу.");

            if (rooms.Select(r => r.RoomId).Distinct().Count() != rooms.Count)
                throw new InvalidOperationException("Одна й та сама зала обрана двічі.");

            foreach (var (roomId, activityId) in rooms)
            {
                var room = await _unitOfWork.RoomRepository.GetByIdAsync(
                    roomId,
                    q => q.Include(r => r.ReservationRooms)
                          .ThenInclude(rr => rr.Reservation)
                );
                if (room == null || !room.IsAvailable)
                    throw new InvalidOperationException($"Кімната з ID {roomId} недоступна.");
                if (room.ReservationRooms != null && room.ReservationRooms.Any(r => r.Reservation != null && r.Reservation.Date.Date == date.Date))
                    throw new InvalidOperationException($"Кімната '{room.Name}' вже заброньована на цю дату.");

                var activity = await _unitOfWork.ActivityRepository.GetByIdAsync(activityId);
                if (activity == null)
                    throw new InvalidOperationException($"Активність з ID {activityId} не існує.");
            }

            var reservation = new Reservation
            {
                CustomerName = customerName,
                Date = date.Date,
                IsEventPackage = isEventPackage,
                AdditionalDetails = additionalDetails,
                ReservationRooms = rooms.Select(x => new ReservationRoom
                {
                    RoomId = x.RoomId,
                    ActivityId = x.ActivityId
                }).ToList()
            };

            await _unitOfWork.ReservationRepository.AddAsync(reservation);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Reservation>> GetUserReservationsAsync(string customerName)
        {
            return await _unitOfWork.ReservationRepository
                .FindAsync(
                    res => res.CustomerName == customerName,
                    q => q
                        .Include(r => r.ReservationRooms)
                            .ThenInclude(rr => rr.Room)
                        .Include(r => r.ReservationRooms)
                            .ThenInclude(rr => rr.Activity)
                );
        }

        public async Task<List<DateTime>> GetBookedDatesAsync()
        {
            var reservations = await _unitOfWork.ReservationRepository.GetAllAsync();
            var rooms = await _unitOfWork.RoomRepository.GetAllAsync();
            int totalRooms = rooms.Count();

            var bookedDates = reservations
                .SelectMany(r => (r.ReservationRooms ?? new List<ReservationRoom>())
                    .Select(rr => new { r.Date, rr.RoomId }))
                .GroupBy(x => x.Date.Date)
                .Where(g => g.Select(x => x.RoomId).Distinct().Count() >= totalRooms)
                .Select(g => g.Key)
                .Distinct()
                .ToList();

            return bookedDates;
        }

        public async Task DeleteReservationAsync(int reservationId)
        {
            var reservation = await _unitOfWork.ReservationRepository.GetByIdAsync(reservationId);
            if (reservation == null)
                throw new KeyNotFoundException("Замовлення не знайдено.");

            await _unitOfWork.ReservationRepository.RemoveAsync(reservation);
            await _unitOfWork.SaveAsync();
        }
    }
}