using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorreoAPIController : ControllerBase
    {
        private readonly BL.Correo _emailService;

        // Inyección del servicio de correo
        public CorreoAPIController(BL.Correo emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("EnviarCorreo")]
        public IActionResult EnviarCorreo(string rutaHTML)
        {
            ML.Result result = new ML.Result();

            try
            {
                string destinatario = "mauricioroma15@hotmail.com"; //Correo que recibe el mensaje
                string asunto = "Test de Envio de Correos"; //Asunto
                string cuerpoHtml = "<h1>Este es un correo de prueba.</h1>" + "<p>Gracias.</p>"; //Plantilla html.

                ////Leo mi archivo html.
                //string cuerpoHtml  = BL.Correo.LeerHTML(rutaHTML); ;

               
                var response = _emailService.EnviarCorreoAsync(destinatario, asunto, cuerpoHtml);

                return Ok("Correo enviado exitosamente.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al enviar la notificación.");
            }
        }
    }
}
