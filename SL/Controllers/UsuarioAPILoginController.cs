using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioAPILoginController : ControllerBase
    {

        //Inyeccion de dependencias de mi BL
        private readonly BL.Usuario _usuarioService;
        private readonly IConfiguration _configuration;
        public UsuarioAPILoginController(BL.Usuario usuarioService, IConfiguration configuration)
        {
            _usuarioService = usuarioService;

            _configuration = configuration;
        }

        // GET api/<UsuarioLoginController>/5

        //[HttpGet("UsuarioAuth")]
        //public ActionResult<ML.Result> Get([FromBody] ML.Usuario usuario)
        //{
        //    ML.Result resultUser = _usuarioService.GetUserByNameAndPassword(usuario);

        //    if(resultUser.Correct == true)
        //    {
        //        return Ok(resultUser);
        //    }
        //    else
        //    {
        //        return BadRequest("No existe el usuario");
        //    }

        //}

        [HttpPost("Login")]
        [AllowAnonymous]
        //[AllowAnonymous] Permite el acceso de los usuarios no autenticados a acciones individuales.

        //[Authorize] Especifica que la clase o el método a los que se aplica este atributo requiere la autorización especificada.
        public IActionResult Login([FromBody] ML.Usuario usuario)
        {
            ML.Result resultUser = _usuarioService.GetUserByNameAndPassword(usuario);

            if (resultUser.Correct == true)
            {
                ML.Usuario usuarioCorrect = (ML.Usuario)resultUser.Object;

                var token = GenerateJwtToken(usuarioCorrect);

                resultUser.Object = token;
                return Ok(resultUser);
            }

            return Unauthorized(new
            {
                Message = "Credenciales incorrectas. Verifique su nombre de usuario y contraseña."
            });
        }

        private string GenerateJwtToken(ML.Usuario usuarioCorrect)
        {
            // Obtener la clave secreta de la configuración
            var jwtKey = _configuration["Jwt:Key"];

            // Obtener Emisor y Audiencia
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioCorrect.IdUsuario.ToString()),
                new Claim(ClaimTypes.Role, usuarioCorrect.Rol.Descripcion)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
