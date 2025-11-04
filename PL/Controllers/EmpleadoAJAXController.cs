using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class EmpleadoAJAXController : Controller
    {
        public IActionResult GetAllAJAX()
        {
            return View();
        }
    }
}

