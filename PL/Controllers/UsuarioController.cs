using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly HttpClient _httpClient;

        public UsuarioController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous] //Permite a cualquier usuario acceder.
        public IActionResult Login(ML.Usuario usuario)
        {
            #region MANDO A LLAMAR MI SERVICIO

            ML.Result resultLoginAPI = new ML.Result();

            //using (var httpClient = new HttpClient()){}

            _httpClient.BaseAddress = new Uri("http://localhost:5186/api/UsuarioAPILogin/");

            //HTTP POST
            var postTask = _httpClient.PostAsJsonAsync<ML.Usuario>("Login", usuario); //Serializar
            postTask.Wait();
            var resultPeticionPost = postTask.Result;

            if (resultPeticionPost.IsSuccessStatusCode)
            {
                var readTask = resultPeticionPost.Content.ReadFromJsonAsync<ML.Result>();
                readTask.Wait();

                // Asigno resultado completo del API a una variable
                ML.Result resultFromApi = readTask.Result;

                resultLoginAPI.Correct = resultFromApi.Correct;

                //Asigno la cadena del token
                resultLoginAPI.Object = resultFromApi.Object;
            }
            else
            {
                resultLoginAPI.Correct = false;
                resultLoginAPI.ErrorMessage = "No existe el usuario";
            }

            #endregion

            if (resultLoginAPI.Correct == true)
            {
                //Obtengo el token
                string token = resultLoginAPI.Object?.ToString()?.Trim('"');

                if (!string.IsNullOrEmpty(token))
                {
                    //Guardo el token en una cookie segura
                    Response.Cookies.Append("JwtToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddMinutes(30)
                    });

                    return RedirectToAction("GetAllSolicitudes", "Permisos");
                }

            }
            else
            {
                ViewBag.ErrorMessage = "Credenciales incorrectas.";
                return View(usuario);
            }

            return View(usuario);
        }

        public IActionResult Logout()
        {
            //Borra la cookie
            Response.Cookies.Delete("JwtToken");

            return RedirectToAction("Login","Usuario");
        }
    }
}
