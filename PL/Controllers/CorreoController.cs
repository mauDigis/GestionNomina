using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace PL.Controllers
{
    //[Authorize]
    public class CorreoController : Controller
    {
        private readonly IHostEnvironment _hostEnvironment;

        private readonly BL.Correo _emailService;

        public CorreoController(IHostEnvironment hostEnvironment, BL.Correo emailService)
        {
            _hostEnvironment = hostEnvironment;

            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Correo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Correo(string emailReceptor)
        {
            //Obtengo mi ruta de mi html
            string filePath = Path.Combine(_hostEnvironment.ContentRootPath, "Img", "PlantillaCorreo.html");

            //Enviar a mi metodo que lo lee
            string archivoHTMLLeido = BL.Correo.LeerHTML(filePath);

            try
            {
                string destinatario = "earredondo@digis01.com"; //Correo que recibe el mensaje
                string asunto = "Test de Envio de Correos"; //Asunto

                //Leo mi archivo html.
                string cuerpoHtml = archivoHTMLLeido;

                //string cuerpoHtml = "<h1>Este es un correo de prueba.</h1>" + "<p>Gracias.</p>"; //Plantilla html.

                var response = _emailService.EnviarCorreoAsync(destinatario, asunto, cuerpoHtml);

                return Ok("Correo enviado exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500,"Error al enviar la notificación." + ex);
            }

        }

    }
}

