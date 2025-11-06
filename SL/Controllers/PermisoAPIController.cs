using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisoAPIController : ControllerBase
    {
        [HttpGet("GetAll")]
        public ActionResult<ML.Result> GetAll()
        {
            ML.Result resultGetAll = BL.Permiso.GetAll();

            if (resultGetAll.Correct == true)
            {
                return Ok(resultGetAll);
            }
            else
            {
                return BadRequest("No se encontraron permisos");
            }
        }

        [HttpPost("Add")]
        public ActionResult<ML.Result> Add(ML.Permiso permiso)
        {
            ML.Result resultAdd = BL.Permiso.Add(permiso);

            if (resultAdd.Correct == true)
            {
                return Ok(resultAdd);
            }
            else
            {
                return BadRequest("No se agrego el permiso");
            }
        }

        [HttpPatch("Update")]
        public ActionResult<ML.Result> Update(ML.Permiso permiso)
        {
            ML.Result resultUpdate = BL.Permiso.Update(permiso);

            if (resultUpdate.Correct == true)
            {
                return Ok(resultUpdate);
            }
            else
            {
                return BadRequest("No se actualizo el permiso");
            }
        }

        [HttpDelete("Delete")]
        public ActionResult<ML.Result> Delete(int IdPermiso)
        {
            ML.Result resultDelete = BL.Permiso.Delete(IdPermiso);

            if (resultDelete.Correct == true)
            {
                return Ok(resultDelete);
            }
            else
            {
                return BadRequest("No se elimino el permisos");
            }
        }

        #region HISTORIAL DE PERMISOS

        [HttpGet("GetAll/HistorialPermisos")]
        public ActionResult<ML.Result> GetAllHistorialPermisos()
        {
            ML.Result resultGetAllHistorial = BL.HistorialPermiso.GetAll();

            if (resultGetAllHistorial.Correct == true)
            {
                return Ok(resultGetAllHistorial);
            }
            else
            {
                return BadRequest("No se encontro ningun historial de permisos");
            }
        }

        #endregion

    }
}
