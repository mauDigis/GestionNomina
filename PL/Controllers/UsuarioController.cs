using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(ML.Usuario usuario)
        {
            #region MANDO A LLAMAR MI SERVICIO

            ML.Result resultLoginAPI = new ML.Result();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5186/api/UsuarioLogin/");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<ML.Usuario>("Login", usuario); //Serializar
                postTask.Wait();
                var resultPeticionPost = postTask.Result;

                if (resultPeticionPost.IsSuccessStatusCode)
                {
                    var readTask = resultPeticionPost.Content.ReadFromJsonAsync<ML.Result>();
                    readTask.Wait();

                    resultLoginAPI.Object = readTask.Result;
                    resultLoginAPI.Correct = true;
                }
                else
                {
                    resultLoginAPI.Correct = false;
                    resultLoginAPI.ErrorMessage = "No existe el usuario";
                }
            }

            #endregion

            if(resultLoginAPI.Correct == true)
            {
                return RedirectToAction("GetAll", "Empleado");
            }
            else
            {
                ViewBag.ErrorMessage = "Credenciales incorrectas.";
                return View(usuario); 
            }

        }
    }
}
