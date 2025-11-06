using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class HistorialPermiso
    {
        public static ML.Result GetAll()
        {
            ML.Result resultGetAllHistorialPermisos = new ML.Result();

            try
            {
                using (DL.GestionNominaContext context = new DL.GestionNominaContext())
                {
                    List<DL.HistorialPermisosDTO> QueryHistorialPermisos = context.HistorialPermisosDTOs.FromSqlInterpolated($"EXECUTE HistorialPermisoGetAll").ToList();

                    if (QueryHistorialPermisos.Count > 0)
                    {
                        resultGetAllHistorialPermisos.Objects = new List<object>();

                        foreach (DL.HistorialPermisosDTO HistorialPermisoOBJ in QueryHistorialPermisos)
                        {
                            ML.HistorialPermiso historialPermiso = new ML.HistorialPermiso();

                            historialPermiso.IdHistorialPermiso = HistorialPermisoOBJ.IdHistorialPermiso;

                            historialPermiso.Permiso = new ML.Permiso();
                            historialPermiso.Permiso.IdPermiso = HistorialPermisoOBJ.IdPermiso;

                            historialPermiso.FechaRevision = HistorialPermisoOBJ.FechaRevision;

                            historialPermiso.StatusPermiso = new ML.StatusPermiso();
                            historialPermiso.StatusPermiso.IdStatusPermiso = HistorialPermisoOBJ.IdStatusPermiso;
                            historialPermiso.StatusPermiso.Descripcion = HistorialPermisoOBJ.NombreStatus;
                           
                            historialPermiso.Observaciones = HistorialPermisoOBJ.Observaciones;

                            historialPermiso.Autoriza = new ML.Empleado();
                            historialPermiso.Autoriza.IdEmpleado =  HistorialPermisoOBJ.IdAutorizador;

                            historialPermiso.Autoriza.Nombre = HistorialPermisoOBJ.NombreAutorizador;
                            historialPermiso.Autoriza.ApellidoPaterno = HistorialPermisoOBJ.ApellidoPatAutorizador;
                            historialPermiso.Autoriza.ApellidoMaterno = HistorialPermisoOBJ.ApellidoMatAutorizador;

                            resultGetAllHistorialPermisos.Objects.Add(historialPermiso);

                        }

                        resultGetAllHistorialPermisos.Correct = true;
                    }
                    else
                    {
                        resultGetAllHistorialPermisos.Correct = false;
                    }
                }

            }
            catch (Exception ex)
            {
                resultGetAllHistorialPermisos.ErrorMessage = ex.Message;
                resultGetAllHistorialPermisos.Exception = ex;
                resultGetAllHistorialPermisos.Correct = false;
            }

            return resultGetAllHistorialPermisos;
        }

    }
}
