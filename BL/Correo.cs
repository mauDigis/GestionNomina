using Microsoft.Extensions.Options;
using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace BL
{
    public class Correo
    {
        private readonly SMTPSettings _smtpSettings;

        // Inyección de la configuración usando IOptions
        public Correo(IOptions<SMTPSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        //Metodo para el envio de Correos
        public async Task EnviarCorreoAsync(string destinatarioEmail, string asunto, string cuerpoHtml)
        {
            // Crear el objeto MailMessage
            using (var message = new MailMessage())
            {
                // Remitente
                message.From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName);

                // Destinatario
                message.To.Add(new MailAddress(destinatarioEmail));

                // Contenido del correo
                message.Subject = asunto;
                message.Body = cuerpoHtml;
                message.IsBodyHtml = true; // Importante para enviar contenido HTML

                // Crea el cliente SMTP -> SmtpClient para enviar el email.
                using (var smtpClient = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port))
                {
                    // Credenciales del correo
                    smtpClient.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);

                    // Configuración de seguridad (TLS/SSL)
                    smtpClient.EnableSsl = _smtpSettings.EnableSsl;

                    // Configuración de timeouts
                    smtpClient.Timeout = 10000; // 10 segundos

                    try
                    {
                        //Enviar el correo
                        await smtpClient.SendMailAsync(message);
                        Console.WriteLine($"Correo enviado a {destinatarioEmail}");
                    }
                    catch (Exception ex)
                    {
                        // Manejo de excepciones de envío (credenciales incorrectas, servidor no disponible)
                        Console.WriteLine($"Error al enviar correo a {destinatarioEmail}: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public static string LeerHTML(string rutaHtml)
        {
            string HtmlLeido = string.Empty;

            using (StreamReader reader = new StreamReader(rutaHtml))
            {
                HtmlLeido = reader.ReadToEnd();
            }

            return HtmlLeido;
        }
    }
}
