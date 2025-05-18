using Cafe.DAL.Repositories;
using Cafe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Cafe.BLL.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return await _unitOfWork.RoomRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime date)
        {
            var rooms = await _unitOfWork.RoomRepository.GetAllAsync(q =>
                q.Include(r => r.ReservationRooms)
                 .ThenInclude(rr => rr.Reservation)
            );

            return rooms.Where(room => room.IsAvailable &&
                (room.ReservationRooms == null || !room.ReservationRooms.Any(res => res.Reservation != null && res.Reservation.Date.Date == date.Date)));
        }
    }
}