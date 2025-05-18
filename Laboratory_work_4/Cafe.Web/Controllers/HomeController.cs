using Cafe.BLL.Facades;
using Microsoft.AspNetCore.Mvc;

namespace Cafe.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly CafeFacade _cafeFacade;

        public HomeController(CafeFacade cafeFacade)
        {
            _cafeFacade = cafeFacade;
        }

        public async Task<IActionResult> Index()
        {
            var availableRooms = await _cafeFacade.GetAvailableRoomsAsync(DateTime.Now);
            return View(availableRooms);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}