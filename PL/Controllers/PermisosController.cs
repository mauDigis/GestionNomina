using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class PermisosController : Controller
    {
        [HttpGet]
        public IActionResult GetAllSolicitudes()
        {
            #region CONSUMO DE SERVICIO GETALL

            ML.Result resultPermiso = new ML.Result();

            resultPermiso.Objects = new List<object>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5186/api/PermisoAPI/");
                var responseTask = client.GetAsync("GetAll");
                responseTask.Wait(); //peticion 
                var result = responseTask.Result; //guardo mi result

                if (result.IsSuccessStatusCode)
                {
                    //var readTask = result.Content.ReadAsAsync<ML.Result>(); 
                    var readTask = result.Content.ReadFromJsonAsync<ML.Result>(); //leo json y lo convierto a result
                    readTask.Wait();

                    foreach (var resultItem in readTask.Result.Objects)
                    {
                        ML.Permiso resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Permiso>(resultItem.ToString()); //deserealiza a mi modelo de Usuario
                        resultPermiso.Objects.Add(resultItemList);
                    }
                    resultPermiso.Correct = true;
                }
                else
                {
                    resultPermiso.Correct = false;
                    resultPermiso.ErrorMessage = "No se encontraron permisos";
                }
            }

            #endregion

            ML.Permiso permiso = new ML.Permiso();

            if (resultPermiso.Correct == true)
            {
                permiso.Permisos = resultPermiso.Objects;
            }

            return View(permiso);
        }

        [HttpGet]
        public IActionResult SolicitudForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SolicitudForm(ML.Permiso permiso)
        {
            // 1. Obtiene el DateTime del día de hoy
            DateTime hoy = DateTime.Today;

            // 2. Convierte el DateTime a DateOnly
            permiso.FechaSolicitud = DateOnly.FromDateTime(hoy);

            permiso.Empleado = new ML.Empleado();
            permiso.Empleado.IdEmpleado = 6;

            permiso.StatusPermiso = new ML.StatusPermiso();
            permiso.StatusPermiso.IdStatusPermiso = 1;

            #region CONSUMO DE SERVICIO ADD

            ML.Result resultAddAPI = new ML.Result();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5186/api/PermisoAPI/");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<ML.Permiso>("Add", permiso); //Serializar
                postTask.Wait();
                var resultPeticionPost = postTask.Result;

                if (resultPeticionPost.IsSuccessStatusCode)
                {
                    var readTask = resultPeticionPost.Content.ReadFromJsonAsync<ML.Result>();
                    readTask.Wait();

                    resultAddAPI.Object = readTask.Result;
                    resultAddAPI.Correct = true;
                }
                else
                {
                    resultAddAPI.Correct = false;
                    resultAddAPI.ErrorMessage = "No se pudo agregar el permiso";
                }
            }

            #endregion

            if (resultAddAPI.Correct == true)
            {
                return RedirectToAction("GetAllSolicitudes");
            }
            return View(permiso);
        }

        public IActionResult ActualizarStatus(int? IdPermiso, byte? IdStatusPermiso)
        {
            ML.Permiso permiso = new ML.Permiso();

            permiso.IdPermiso = IdPermiso;
            permiso.StatusPermiso = new ML.StatusPermiso();
            permiso.StatusPermiso.IdStatusPermiso = IdStatusPermiso;
            
            if(IdStatusPermiso == 2) //Aprobado
            {
                permiso.IdAutorizador = 7;
            }
            else //Rechazado
            {
                permiso.IdAutorizador = 18;
            }

            #region CONSUMO DE SERVICIO UPDATE

            ML.Result resultUpdateAPI = new ML.Result();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5186/api/PermisoAPI/");

                //HTTP Patch
                var postTask = client.PatchAsJsonAsync<ML.Permiso>("Update", permiso); //Serializar
                postTask.Wait();
                var resultPeticionPost = postTask.Result;

                if (resultPeticionPost.IsSuccessStatusCode)
                {
                    var readTask = resultPeticionPost.Content.ReadFromJsonAsync<ML.Result>();
                    readTask.Wait();

                    resultUpdateAPI.Object = readTask.Result;
                    resultUpdateAPI.Correct = true;

                }
                else
                {
                    resultUpdateAPI.Correct = false;
                    resultUpdateAPI.ErrorMessage = "No se pudo actualizar el registro.";
                }
            }

            #endregion

            if (resultUpdateAPI.Correct == true)
            {
                return RedirectToAction("GetAllSolicitudes");
            }

            return RedirectToAction("GetAllSolicitudes");

        }

        [HttpGet]
        public IActionResult HistorialPermisos()
        {
            #region CONSUMO DE SERVICIO GETALLHistorialPermisos

            ML.Result resultHistorialPermisos = new ML.Result();

            resultHistorialPermisos.Objects = new List<object>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5186/api/PermisoAPI/");
                var responseTask = client.GetAsync("GetAll/HistorialPermisos");
                responseTask.Wait(); //peticion 
                var result = responseTask.Result; //guardo mi result

                if (result.IsSuccessStatusCode)
                { 
                    var readTask = result.Content.ReadFromJsonAsync<ML.Result>(); //leo json y lo convierto a result
                    readTask.Wait();

                    foreach (var resultItem in readTask.Result.Objects)
                    {
                        ML.HistorialPermiso resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.HistorialPermiso>(resultItem.ToString()); //deserealiza a mi modelo de Usuario
                        resultHistorialPermisos.Objects.Add(resultItemList);
                    }
                    resultHistorialPermisos.Correct = true;
                }
                else
                {
                    resultHistorialPermisos.Correct = false;
                    resultHistorialPermisos.ErrorMessage = "No se encontro historial de permisos.";
                }
            }

            #endregion

            ML.HistorialPermiso historialPermiso = new ML.HistorialPermiso();   

            if (resultHistorialPermisos.Correct == true)
            {
                historialPermiso.ListaHistorialPermisos = resultHistorialPermisos.Objects;
            }

            return View(historialPermiso);
        }

    }
}
