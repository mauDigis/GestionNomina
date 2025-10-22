using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Empleado
    {
        //Stored Procedure
        public static ML.Result GetAll()
        {
            ML.Result resultGetAll = new ML.Result();

            try
            {
                using (DL.GestionNominaContext context = new DL.GestionNominaContext())
                {
                    //Se utiliza mi DTO, para mapear los datos que retorna mi stored a mis modelos.
                    var resultEmpleadoList = context.EmpleadosDTOs.FromSqlInterpolated($"EXECUTE EmpleadoGetAll")
                        .ToList();

                    if (resultEmpleadoList.Count > 0) 
                    {
                        resultGetAll.Objects = new List<object>();

                        foreach ( var empleadoObj in resultEmpleadoList)
                        {
                            ML.Empleado empleadoML = new ML.Empleado();

                            empleadoML.IdEmpleado =  empleadoObj.IdEmpleado;
                            empleadoML.Nombre = empleadoObj.Nombre;
                            empleadoML.ApellidoPaterno = empleadoObj.ApellidoPaterno;
                            empleadoML.ApellidoMaterno = empleadoObj.ApellidoMaterno;
                            empleadoML.FechaNacimiento = empleadoObj.FechaNacimiento;
                            empleadoML.RFC = empleadoObj.RFC;
                            empleadoML.NSS = empleadoObj.NSS;
                            empleadoML.CURP = empleadoObj.CURP;
                            empleadoML.FechaIngreso = empleadoObj.FechaIngreso;

                            empleadoML.Departamento = new ML.Departamento();
                            empleadoML.Departamento.IdDepartamento = empleadoObj.IdDepartamento;
                            empleadoML.Departamento.Descripcion = empleadoObj.Descripcion;

                            empleadoML.SalarioBase = empleadoObj.SalarioBase;
                            empleadoML.NoFaltas = empleadoObj.NoFaltas;

                            resultGetAll.Objects.Add(empleadoML);
                        }

                        resultGetAll.Correct = true;
                    }
                    else
                    {
                        resultGetAll.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                resultGetAll.ErrorMessage = ex.Message;
                resultGetAll.Correct = false;
                resultGetAll.Exception = ex;
            }

            return resultGetAll;
        }
        public static ML.Result GetById(int IdEmpleado)
        {
            ML.Result resultGetById = new ML.Result();

            try
            {
                using(DL.GestionNominaContext context = new DL.GestionNominaContext())
                {
                    var queryGetEmpleado = (from EmpleadoDb in context.Empleados
                                           join DepartamentoDb in context.Departamentos on EmpleadoDb.IdDepartamento equals DepartamentoDb.IdDepartamento
                                           where EmpleadoDb.IdEmpleado == IdEmpleado
                                           select new
                                           {
                                               EmpleadoDb.IdEmpleado,
                                               EmpleadoDb.Nombre,
                                               EmpleadoDb.ApellidoPaterno,
                                               EmpleadoDb.ApellidoMaterno,
                                               EmpleadoDb.FechaNacimiento,
                                               EmpleadoDb.Rfc,
                                               EmpleadoDb.Nss,
                                               EmpleadoDb.Curp,
                                               EmpleadoDb.FechaIngreso,
                                               EmpleadoDb.IdDepartamento,
                                               EmpleadoDb.SalarioBase,
                                               EmpleadoDb.NoFaltas,
                                               DepartamentoDb.Descripcion
                                           }).SingleOrDefault();

                    if (queryGetEmpleado != null)
                    {
                        ML.Empleado empleadoML = new ML.Empleado();

                        empleadoML.IdEmpleado = queryGetEmpleado.IdEmpleado;
                        empleadoML.Nombre = queryGetEmpleado.Nombre;
                        empleadoML.ApellidoPaterno = queryGetEmpleado.ApellidoPaterno;
                        empleadoML.ApellidoMaterno = queryGetEmpleado.ApellidoMaterno;
                        empleadoML.FechaNacimiento = queryGetEmpleado.FechaNacimiento;
                        empleadoML.RFC = queryGetEmpleado.Rfc;
                        empleadoML.NSS = queryGetEmpleado.Nss;
                        empleadoML.CURP = queryGetEmpleado.Curp;
                        empleadoML.FechaIngreso = queryGetEmpleado.FechaIngreso;

                        empleadoML.Departamento = new ML.Departamento();
                        empleadoML.Departamento.IdDepartamento = queryGetEmpleado.IdDepartamento;
                        empleadoML.Departamento.Descripcion = queryGetEmpleado.Descripcion;

                        empleadoML.SalarioBase = queryGetEmpleado.SalarioBase;
                        empleadoML.NoFaltas = queryGetEmpleado.NoFaltas;

                        resultGetById.Object = empleadoML;

                        if (resultGetById.Object != null)
                        {
                            resultGetById.Correct = true;
                        }
                    }
                    else
                    {
                        resultGetById.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                resultGetById.ErrorMessage = ex.Message; 
                resultGetById.Correct = false;
                resultGetById.Exception = ex;
            }

            return resultGetById;
        }
        public static ML.Result Add(ML.Empleado empleado)
        {
            ML.Result resultAdd = new ML.Result();

            try
            {
                using (DL.GestionNominaContext context = new DL.GestionNominaContext())
                {
                    int QueryEmpleadoAdd = context.Database.ExecuteSqlInterpolated($@"
                        EXECUTE EmpleadoAdd
                            @Nombre = {empleado.Nombre},
                            @ApellidoPaterno = {empleado.ApellidoPaterno},
                            @ApellidoMaterno = {empleado.ApellidoMaterno},
                            @FechaNacimiento = {empleado.FechaNacimiento},
                            @RFC             = {empleado.RFC},
                            @NSS             = {empleado.NSS},
                            @CURP            = {empleado.CURP},
                            @FechaIngreso    = {empleado.FechaIngreso},
                            @IdDepartamento  = {empleado.Departamento.IdDepartamento}, 
                            @SalarioBase     = {empleado.SalarioBase},
                            @NoFaltas        = {empleado.NoFaltas}
                    ");

                    if (QueryEmpleadoAdd > 0)
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
        public static ML.Result Update(ML.Empleado empleado)
        {
            ML.Result resultUpdEmpleado = new ML.Result();

            try
            {
                using(DL.GestionNominaContext context = new DL.GestionNominaContext())
                {
                    var queryUpdEmp = (from EmpleadoDb in context.Empleados
                                       where EmpleadoDb.IdEmpleado == empleado.IdEmpleado
                                       select EmpleadoDb).SingleOrDefault();

                    if(queryUpdEmp != null)
                    {
                        queryUpdEmp.Nombre = empleado.Nombre;
                        queryUpdEmp.ApellidoPaterno = empleado.ApellidoPaterno;
                        queryUpdEmp.ApellidoMaterno = empleado.ApellidoMaterno;
                        queryUpdEmp.FechaNacimiento = empleado.FechaNacimiento;
                        queryUpdEmp.Rfc = empleado.RFC;
                        queryUpdEmp.Nss = empleado.NSS;
                        queryUpdEmp.Curp = empleado.CURP;
                        queryUpdEmp.FechaIngreso = empleado.FechaIngreso;
                        queryUpdEmp.IdDepartamento = empleado.Departamento.IdDepartamento;
                        queryUpdEmp.SalarioBase = empleado.SalarioBase;
                        queryUpdEmp.NoFaltas = empleado.NoFaltas;

                        int RowsAffected = context.SaveChanges();

                        if(RowsAffected > 0)
                        {
                            resultUpdEmpleado.Correct = true;
                        }
                        else
                        {
                            resultUpdEmpleado.Correct = false;
                        }
                    }
                }

            }catch (Exception ex)
            {
                resultUpdEmpleado.ErrorMessage = ex.Message;
                resultUpdEmpleado.Correct = false;
                resultUpdEmpleado.Exception = ex;
            }

            return resultUpdEmpleado;
        }
        public static ML.Result Delete(int IdEmpleado)
        {
            ML.Result resultDeleteEmp = new ML.Result();

            try
            {
                using (DL.GestionNominaContext context = new DL.GestionNominaContext())
                {
                    var resultQuery = (from EmpleadoDb in context.Empleados
                                       where EmpleadoDb.IdEmpleado == IdEmpleado
                                       select EmpleadoDb).SingleOrDefault();

                    context.Empleados.Remove(resultQuery);

                    int filasAfectadas = context.SaveChanges();

                    if (filasAfectadas > 0)
                    {
                        resultDeleteEmp.Correct = true;
                    }
                    else
                    {
                        resultDeleteEmp.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                resultDeleteEmp.Correct = false;
                resultDeleteEmp.ErrorMessage = ex.Message;
            }

            return resultDeleteEmp;
        }
    }
}
