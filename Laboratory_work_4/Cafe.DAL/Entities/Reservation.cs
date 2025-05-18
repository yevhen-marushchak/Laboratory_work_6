using System;
using System.Collections.Generic;

namespace Cafe.DAL.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime Date { get; set; }
        public bool IsEventPackage { get; set; }
        public string? AdditionalDetails { get; set; }

        public ICollection<ReservationRoom> ReservationRooms { get; set; }
    }

    public class ReservationRoom
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public int ActivityId { get; set; }
        public Activity Activity { get; set; }
    }
}