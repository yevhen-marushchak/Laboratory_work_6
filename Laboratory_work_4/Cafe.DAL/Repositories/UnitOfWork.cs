using Cafe.DAL.Entities;
using System;
using System.Threading.Tasks;

namespace Cafe.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CafeDbContext _context;
        private IGenericRepository<Activity> _activityRepository;
        private IGenericRepository<Room> _roomRepository;
        private IGenericRepository<Reservation> _reservationRepository;

        public UnitOfWork(CafeDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<Activity> ActivityRepository
        {
            get
            {
                if (_activityRepository == null)
                {
                    _activityRepository = new GenericRepository<Activity>(_context);
                }
                return _activityRepository;
            }
        }

        public IGenericRepository<Room> RoomRepository
        {
            get
            {
                if (_roomRepository == null)
                {
                    _roomRepository = new GenericRepository<Room>(_context);
                }
                return _roomRepository;
            }
        }

        public IGenericRepository<Reservation> ReservationRepository
        {
            get
            {
                if (_reservationRepository == null)
                {
                    _reservationRepository = new GenericRepository<Reservation>(_context);
                }
                return _reservationRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}