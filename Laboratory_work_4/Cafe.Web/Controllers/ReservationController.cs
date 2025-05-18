using Cafe.BLL.Facades;
using Cafe.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Cafe.Web.Controllers
{
    public class ReservationController : Controller
    {
        private readonly CafeFacade _cafeFacade;

        public ReservationController(CafeFacade cafeFacade)
        {
            _cafeFacade = cafeFacade;
        }

        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetString("User") == null)
                return RedirectToAction("Login", "Account");

            var rooms = (await _cafeFacade.GetAllRoomsAsync()).ToList();
            var bookedDates = await _cafeFacade.GetBookedDatesAsync();

            var model = new ReservationViewModel
            {
                Rooms = rooms,
                BookedDates = bookedDates
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReservationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var selectedRooms = model.SelectedRooms
                        .Where(x => x.RoomId.HasValue && x.ActivityId.HasValue)
                        .Select(x => (x.RoomId.Value, x.ActivityId.Value))
                        .ToList();

                    await _cafeFacade.CreateReservationAsync(
                        HttpContext.Session.GetString("User"),
                        model.Date.Value,
                        model.IsEventPackage,
                        model.AdditionalDetails,
                        selectedRooms);

                    return RedirectToAction("MyReservations");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            model.Rooms = (await _cafeFacade.GetAllRoomsAsync()).ToList();
            return View(model);
        }

        public async Task<IActionResult> MyReservations()
        {
            if (HttpContext.Session.GetString("User") == null)
                return RedirectToAction("Login", "Account");

            var reservations = await _cafeFacade.GetUserReservationsAsync(HttpContext.Session.GetString("User"));
            return View(reservations);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _cafeFacade.DeleteReservationAsync(id);
            return RedirectToAction("MyReservations");
        }
    }
}