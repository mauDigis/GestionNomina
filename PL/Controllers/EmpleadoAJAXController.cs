using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class EmpleadoAJAXController : Controller
    {
        public IActionResult GetAll()
        {
            return View();
        }

        public IActionResult Form()
        {
            return View();
        }
    }
}
