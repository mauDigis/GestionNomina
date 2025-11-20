using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] Permite acceder mientras el usuario tenga un token, independiente de su rol.
    public class EmpleadoAPIController : ControllerBase
    {
        [HttpGet("GetAll")]
        [Authorize(Roles = "Administrador")]
        public ActionResult<ML.Result> GetAll()
        {
            ML.Result resultGetAll = BL.Empleado.GetAll();

            if (resultGetAll.Correct == true)
            {
                return Ok(resultGetAll);
            }
            else
            {
                return BadRequest("No se encontraron empleados");
            }
        }

        [HttpGet("GetById/{IdEmpleado}")]
        public ActionResult<ML.Result> GetById(int IdEmpleado)
        {
            ML.Result resultGetById = BL.Empleado.GetById(IdEmpleado);

            if (resultGetById.Correct == true)
            {
                return Ok(resultGetById);
            }
            else
            {
                return BadRequest("No se encontro el empleado verifica el id" + resultGetById);
            }
        }

        [HttpPost("Add")]
        public ActionResult<ML.Result> Add(ML.Empleado empleado)
        {
            ML.Result resultAdd = BL.Empleado.Add(empleado);

            if (resultAdd.Correct == true)
            {
                return Ok(resultAdd);
            }
            else
            {
                return BadRequest("No se pudo agregar el usuario" + resultAdd);
            }
        }

        [HttpPut("Update")]
        public ActionResult<ML.Result> Update(ML.Empleado empleado)
        {
            ML.Result resultUpdate = BL.Empleado.Update(empleado);

            if (resultUpdate.Correct == true)
            {
                return Ok(resultUpdate);
            }
            else
            {
                return BadRequest("No se pudo actualizar el usuario" + resultUpdate);
            }
        }

        [HttpDelete("Delete/{IdEmpleado}")]
        public ActionResult<ML.Result> Delete(int IdEmpleado)
        {
            ML.Result resultDelete = BL.Empleado.Delete(IdEmpleado);

            if (resultDelete.Correct == true)
            {
                return Ok(resultDelete);
            }
            else
            {
                return BadRequest("No se pudo eliminar el empleado verifica el id" + resultDelete);
            }
        }
    }
}
