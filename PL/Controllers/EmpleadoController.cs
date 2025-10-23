using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly IWebHostEnvironment _env;

        // 2. Constructor para realizar la inyección de dependencias
        public EmpleadoController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Empleado empleado = new ML.Empleado();

            //ML.Result resultGetAll = BL.Empleado.GetAll();

            ML.Result resultGetAll = BL.Empleado.GetAllAPI();

            if (resultGetAll.Correct == true) 
            {
                empleado.Empleados = resultGetAll.Objects;
            }
            else
            {

            }

            return View(empleado);
        }

        [HttpGet]
        public IActionResult Form(int? IdEmpleado) 
        {
            ML.Empleado empleado = new ML.Empleado();

            if(IdEmpleado > 0) //Consulto mi usuario
            {
                ML.Result resultGetById = BL.Empleado.GetByIdAPI(IdEmpleado.Value);

                if (resultGetById.Correct == true)
                {
                    empleado = (ML.Empleado)resultGetById.Object;//Unboxing
                    ViewBag.Message = "Datos de Empleado cargados correctamente.";
                }
                else
                {
                    ViewBag.Message = "Error: No se pudo cargar el empleado solicitado.";
                }
            }
            else //Retorno la vista
            {
                
            }

            return View(empleado);
        }

        [HttpPost]
        public IActionResult Form(ML.Empleado empleado, IFormFile ImageFile)
        {
            // Verificar si se subió un archivo
            if (ImageFile != null)
            {
                // Convertir IFormFile a byte[]
                using (var ms = new System.IO.MemoryStream())
                {
                    ImageFile.CopyTo(ms);
                    empleado.Imagen = ms.ToArray(); 
                }
            }
            else
            {
                //Obtengo la ruta de mi imagen por defecto.
                var webRootPath = _env.ContentRootPath; 
                var defaultImagePath = Path.Combine(webRootPath, "wwwroot", "Img", "NoPhoto.png");

                if (System.IO.File.Exists(defaultImagePath))
                {
                    // Leer el archivo de imagen por defecto y convertirlo a byte[]
                    empleado.Imagen = System.IO.File.ReadAllBytes(defaultImagePath);
                }
            }

            if (empleado.IdEmpleado == 0) //Agrego
            {
                ML.Result resultAdd = BL.Empleado.AddAPI(empleado);

                if (resultAdd.Correct == true)
                {
                    return RedirectToAction("GetAll");
                }
                else
                {
                    ViewBag.Message = "Error: No se pudo guardar el empleado.";
                }
            }
            else //Actualizo
            {
                ML.Result resultUpdate = BL.Empleado.UpdateAPI(empleado);

                if(resultUpdate.Correct == true)
                {
                    return RedirectToAction("GetAll");
                }
                else
                {
                    ViewBag.Message = "Error: No se pudo actualizar el empleado solicitado.";
                }
            }

            return RedirectToAction("GetAll");
        }

        public IActionResult Delete(int IdEmpleado)
        {

            ML.Result resultDeleteEmpleado = BL.Empleado.DeleteAPI(IdEmpleado);

            if (resultDeleteEmpleado.Correct == true)
            {
                return RedirectToAction("GetAll");
            }
            else
            {

            }

            return RedirectToAction("GetAll");

        }
    }
}
