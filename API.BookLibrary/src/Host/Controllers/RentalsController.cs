using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    public class RentalsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
