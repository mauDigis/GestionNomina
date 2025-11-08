using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioLoginController : ControllerBase
    {

        //Inyeccion de dependencias de mi BL
        private readonly BL.Usuario _usuarioService;
        private readonly IConfiguration _configuration;
        public UsuarioLoginController(BL.Usuario usuarioService, IConfiguration configuration)
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
        public IActionResult Login([FromBody] ML.Usuario usuario)
        {
            ML.Result resultUser = _usuarioService.GetUserByNameAndPassword(usuario);

            if (resultUser.Correct == true)
            {
                ML.Usuario usuarioCorrect = (ML.Usuario)resultUser.Object;

                if (usuarioCorrect.Rol.Descripcion == "Administrador" && usuarioCorrect.PasswordHash == "oroblanco")
                {
                    var token = GenerateJwtToken(usuarioCorrect.NombreUsuario, usuarioCorrect.PasswordHash, usuarioCorrect.Rol.Descripcion);
                    return Ok(new { token });
                }
            }

            return Unauthorized();
        }

        private string GenerateJwtToken(string NombreUsuario, string PasswordHash, string RolDescripcion)
        {

            // Obtener la clave secreta de la configuración
            var jwtKey = _configuration["Jwt:Key"];

            // Obtener Emisor y Audiencia
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, NombreUsuario),
            new Claim(ClaimTypes.Name, PasswordHash),
            new Claim(ClaimTypes.Role, RolDescripcion),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
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
