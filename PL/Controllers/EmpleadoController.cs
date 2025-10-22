using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class EmpleadoController : Controller
    {
        public IActionResult GetAll()
        {
            ML.Empleado empleado = new ML.Empleado();

            ML.Result resultGetAll = BL.Empleado.GetAll();

            if (resultGetAll.Correct == true) 
            {
                empleado.Empleados = resultGetAll.Objects;
            }
            else
            {

            }

            return View(empleado);
        }
    }
}
