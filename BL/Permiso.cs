using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BL
{
    public class Permiso
    {
        public static ML.Result GetAll()
        {
            ML.Result resultGetAllPermisos = new ML.Result();

            try
            {
                using (DL.GestionNominaContext context = new DL.GestionNominaContext())
                {
                    List<DL.PermisosDTOs> QueryPermisos = context.PermisosDTOs.FromSqlInterpolated($"EXECUTE PermisoGetAll").ToList();

                    if (QueryPermisos.Count > 0)
                    {
                        resultGetAllPermisos.Objects = new List<object>();

                        foreach (DL.PermisosDTOs PermisoOBJ in QueryPermisos)
                        {
                            ML.Permiso permisoML = new ML.Permiso();

                            permisoML.IdPermiso = PermisoOBJ.IdPermiso;

                            permisoML.Empleado = new ML.Empleado();
                            permisoML.Empleado.IdEmpleado = PermisoOBJ.IdEmpleado;

                            permisoML.FechaSolicitud = PermisoOBJ.FechaSolicitud;
                            permisoML.FechaInicio = PermisoOBJ.FechaInicio;
                            permisoML.FechaFin = PermisoOBJ.FechaFin;
                            permisoML.HoraInicio = PermisoOBJ.HoraInicio;
                            permisoML.HoraFin = PermisoOBJ.HoraFin;
                            permisoML.Motivo = PermisoOBJ.Motivo;

                            permisoML.StatusPermiso = new ML.StatusPermiso();
                            permisoML.StatusPermiso.IdStatusPermiso = PermisoOBJ.IdStatusPermiso;

                            permisoML.IdAutorizador = PermisoOBJ.IdAutorizador;

                            if (permisoML.IdAutorizador.HasValue) // Verifica si hay un autorizador
                            {
                                // 1. Llama al método para obtener el Empleado Autorizador por su ID

                                ML.Result resultAutoriza = BL.Permiso.GetByIdAutoriza(permisoML.IdAutorizador.Value);

                                if (resultAutoriza.Correct == true && resultAutoriza.Object != null)
                                {
                                    // 2. Castea el objeto resultado de ML.Result a (ML.Permiso)
                                    ML.Permiso permisoConAutorizador = (ML.Permiso)resultAutoriza.Object;

                                    // 3. ¡Asigna la propiedad Autoriza al permiso actual!
                                    permisoML.Autoriza = permisoConAutorizador.Autoriza;
                                }
                            }

                            permisoML.Empleado.Nombre = PermisoOBJ.Nombre;
                            permisoML.Empleado.ApellidoPaterno = PermisoOBJ.ApellidoPaterno;
                            permisoML.Empleado.ApellidoMaterno = PermisoOBJ.ApellidoMaterno;

                            permisoML.StatusPermiso = new ML.StatusPermiso();
                            permisoML.StatusPermiso.Descripcion = PermisoOBJ.NombreStatus;

                            permisoML.Departamento = new ML.Departamento();
                            permisoML.Departamento.Descripcion = PermisoOBJ.NombreDepartamento;

                            resultGetAllPermisos.Objects.Add(permisoML);

                        }

                        resultGetAllPermisos.Correct = true;
                    }
                    else
                    {
                        resultGetAllPermisos.Correct = false;
                    }
                }

            }
            catch (Exception ex)
            {
                resultGetAllPermisos.ErrorMessage = ex.Message;
                resultGetAllPermisos.Exception = ex;
                resultGetAllPermisos.Correct = false;
            }

            return resultGetAllPermisos;
        }
        public static ML.Result GetById(int IdPermiso)
        {
            ML.Result resultGetByIdPermisos = new ML.Result();

            try
            {
                using (DL.GestionNominaContext context = new DL.GestionNominaContext())
                {
                    var QueryPermiso = (from PermisosDb in context.Permisos
                                        join EmpleadoDb in context.Empleados on PermisosDb.IdEmpleado equals EmpleadoDb.IdEmpleado
                                        join StatusPermisoDb in context.StatusPermisos on PermisosDb.IdStatusPermiso equals StatusPermisoDb.IdStatusPermiso
                                        where PermisosDb.IdPermiso == IdPermiso
                                        select new
                                        {
                                            PermisosDb.IdPermiso,
                                            PermisosDb.IdEmpleado,
                                            PermisosDb.FechaSolicitud,
                                            PermisosDb.FechaInicio,
                                            PermisosDb.FechaFin,
                                            PermisosDb.HoraInicio,
                                            PermisosDb.HoraFin,
                                            PermisosDb.Motivo,
                                            PermisosDb.IdStatusPermiso,
                                            PermisosDb.IdAutorizador,
                                            EmpleadoDb.Nombre,
                                            EmpleadoDb.ApellidoPaterno,
                                            EmpleadoDb.ApellidoMaterno,
                                            StatusPermisoDb.Descripcion,
                                        }).SingleOrDefault();

                    if (QueryPermiso != null)
                    {
                        ML.Permiso permisoML = new ML.Permiso();

                        permisoML.IdPermiso = QueryPermiso.IdPermiso;

                        permisoML.Empleado = new ML.Empleado();
                        permisoML.Empleado.IdEmpleado = QueryPermiso.IdEmpleado;
                        permisoML.FechaSolicitud = QueryPermiso.FechaSolicitud;
                        permisoML.FechaInicio = QueryPermiso.FechaInicio;
                        permisoML.FechaFin = QueryPermiso.FechaFin;
                        permisoML.HoraInicio = QueryPermiso.HoraInicio;
                        permisoML.HoraFin = QueryPermiso.HoraFin;
                        permisoML.Motivo = QueryPermiso.Motivo;
                        permisoML.StatusPermiso.IdStatusPermiso = QueryPermiso.IdStatusPermiso;
                        permisoML.IdAutorizador = QueryPermiso.IdAutorizador;

                        resultGetByIdPermisos.Correct = true;
                    }
                    else
                    {
                        resultGetByIdPermisos.Correct = false;
                    }
                }

            }
            catch (Exception ex)
            {
                resultGetByIdPermisos.ErrorMessage = ex.Message;
                resultGetByIdPermisos.Exception = ex;
                resultGetByIdPermisos.Correct = false;
            }

            return resultGetByIdPermisos;
        }
        public static ML.Result Add(ML.Permiso permiso)
        {
            ML.Result resultAdd = new ML.Result();

            try
            {
                using (DL.GestionNominaContext context = new DL.GestionNominaContext())
                {
                    int QueryPermisoAdd = context.Database.ExecuteSqlInterpolated($@"
                          EXECUTE PermisoAdd
                               @IdEmpleado = {permiso.Empleado.IdEmpleado},
                               @FechaSolicitud = {permiso.FechaSolicitud},
                               @FechaInicio = {permiso.FechaInicio},
                               @FechaFIN = {permiso.FechaFin},
                               @HoraInicio = {permiso.HoraInicio},
                               @HoraFin = {permiso.HoraFin},
                               @Motivo = {permiso.Motivo},
                               @IdStatusPermiso = {permiso.StatusPermiso.IdStatusPermiso},
                               @IdAutorizador = {permiso.IdAutorizador}
                    ");

                    if (QueryPermisoAdd > 0)
                    {
                        resultAdd.Correct = true;
                    }
                    else
                    {
                        resultAdd.Correct = false;
                    }
                }

            }
            catch (Exception ex)
            {
                resultAdd.ErrorMessage = ex.Message;
                resultAdd.Correct = false;
                resultAdd.Exception = ex;
            }

            return resultAdd;
        }
        public static ML.Result Update(ML.Permiso permiso)
        {
            ML.Result resultUpdPermiso = new ML.Result();

            try
            {
                using (DL.GestionNominaContext context = new DL.GestionNominaContext())
                {
                    var queryUpdPermiso = (from PermisoDb in context.Permisos
                                           where PermisoDb.IdPermiso == permiso.IdPermiso
                                           select PermisoDb).SingleOrDefault();

                    if (queryUpdPermiso != null)
                    {
                        queryUpdPermiso.IdStatusPermiso = permiso.StatusPermiso.IdStatusPermiso;
                        queryUpdPermiso.IdAutorizador = permiso.IdAutorizador;

                        int RowsAffected = context.SaveChanges();

                        if (RowsAffected > 0)
                        {
                            //Mandar a llamar mi stored procedure de HistorialPermiso
                            int QueryHistorialUpd = context.Database.ExecuteSqlInterpolated($@"
                             EXECUTE HistorialPermisoUpdate
                               @IdPermiso = {permiso.IdPermiso},
                               @NuevoStatusId = {queryUpdPermiso.IdStatusPermiso},
                               @NuevoAutorizadorId = {queryUpdPermiso.IdAutorizador}
                            ");

                            if(QueryHistorialUpd > 0)
                            {
                                resultUpdPermiso.Correct = true;
                            }
                        }
                        else
                        {
                            resultUpdPermiso.Correct = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                resultUpdPermiso.ErrorMessage = ex.Message;
                resultUpdPermiso.Correct = false;
                resultUpdPermiso.Exception = ex;
            }

            return resultUpdPermiso;
        }
        public static ML.Result Delete(int IdPermiso)
        {
            ML.Result resultDeletePermiso = new ML.Result();

            try
            {
                using (DL.GestionNominaContext context = new DL.GestionNominaContext())
                {
                    var resultQuery = (from PermisoDb in context.Permisos
                                       where PermisoDb.IdPermiso == IdPermiso
                                       select PermisoDb).SingleOrDefault();

                    context.Permisos.Remove(resultQuery);

                    int filasAfectadas = context.SaveChanges();

                    if (filasAfectadas > 0)
                    {
                        resultDeletePermiso.Correct = true;
                    }
                    else
                    {
                        resultDeletePermiso.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                resultDeletePermiso.Correct = false;
                resultDeletePermiso.ErrorMessage = ex.Message;
            }

            return resultDeletePermiso;
        }
        public static ML.Result GetByIdAutoriza(int IdAutorizador)
        {
            ML.Result resultGetByIdAutoriza = new ML.Result();

            try
            {
                using (DL.GestionNominaContext context = new DL.GestionNominaContext())
                {
                    var QueryAutorizador = (from EmpleadoDb in context.Empleados
                                            where EmpleadoDb.IdEmpleado == IdAutorizador
                                            select new
                                            {
                                                EmpleadoDb.Nombre,
                                                EmpleadoDb.ApellidoPaterno,
                                                EmpleadoDb.ApellidoMaterno
                                            }).SingleOrDefault();

                    if (QueryAutorizador != null)
                    {
                        ML.Permiso permisoML = new ML.Permiso();

                        permisoML.Autoriza = new ML.Empleado();
                        permisoML.Autoriza.Nombre = QueryAutorizador.Nombre;
                        permisoML.Autoriza.ApellidoPaterno = QueryAutorizador.ApellidoPaterno;
                        permisoML.Autoriza.ApellidoMaterno = QueryAutorizador.ApellidoMaterno;

                        resultGetByIdAutoriza.Object = permisoML;
                        resultGetByIdAutoriza.Correct = true;
                    }
                    else
                    {
                        resultGetByIdAutoriza.Correct = false;
                    }
                }

            }
            catch (Exception ex)
            {
                resultGetByIdAutoriza.ErrorMessage = ex.Message;
                resultGetByIdAutoriza.Exception = ex;
                resultGetByIdAutoriza.Correct = false;
            }

            return resultGetByIdAutoriza;
        }
    }
}
