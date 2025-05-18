using System.Collections.Generic;

namespace Cafe.DAL.Entities
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}