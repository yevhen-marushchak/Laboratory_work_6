using Cafe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cafe.BLL.Services
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetAllRoomsAsync();
        Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime date);
    }
}