using Cafe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cafe.Web.Models
{
    public class ReservationRoomSelection
    {
        public int? RoomId { get; set; }
        public int? ActivityId { get; set; }
    }

    public class ReservationViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Дата є обов'язковою.")]
        public DateTime? Date { get; set; }

        public bool IsEventPackage { get; set; }

        public string? AdditionalDetails { get; set; }

        public List<Room> Rooms { get; set; } = new();
        public List<ReservationRoomSelection> SelectedRooms { get; set; } = new();
        public List<DateTime> BookedDates { get; set; } = new();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var selected = SelectedRooms?.FindAll(r => r.RoomId.HasValue) ?? new List<ReservationRoomSelection>();

            if (selected.Count == 0)
                yield return new ValidationResult("Потрібно вибрати хоча б одну залу.", new[] { nameof(SelectedRooms) });

            if (selected.Count > 2)
                yield return new ValidationResult("Можна вибрати не більше двох залів.", new[] { nameof(SelectedRooms) });

            for (int i = 0; i < selected.Count; i++)
            {
                if (!selected[i].ActivityId.HasValue)
                    yield return new ValidationResult($"Оберіть активність для зали №{i + 1}.", new[] { $"SelectedRooms[{i}].ActivityId" });
            }

            if (IsEventPackage)
            {
                if (string.IsNullOrWhiteSpace(AdditionalDetails))
                    yield return new ValidationResult("Вкажіть додаткові відомості для заходу під ключ.", new[] { nameof(AdditionalDetails) });
            }
        }
    }
}