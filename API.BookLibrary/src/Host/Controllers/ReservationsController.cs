using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    public class ReservationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
