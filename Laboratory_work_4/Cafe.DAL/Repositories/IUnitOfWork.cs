using Cafe.DAL.Entities;
using System;
using System.Threading.Tasks;

namespace Cafe.DAL.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Activity> ActivityRepository { get; }
        IGenericRepository<Room> RoomRepository { get; }
        IGenericRepository<Reservation> ReservationRepository { get; }
        Task SaveAsync();
    }
}