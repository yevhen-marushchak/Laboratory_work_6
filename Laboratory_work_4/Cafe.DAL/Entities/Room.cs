using System.Collections.Generic;

namespace Cafe.DAL.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAvailable { get; set; }
        public ICollection<ReservationRoom> ReservationRooms { get; set; }
        public ICollection<Activity> Activities { get; set; }
    }
}