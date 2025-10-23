using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

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

                        foreach (var empleadoObj in resultEmpleadoList)
                        {
                            ML.Empleado empleadoML = new ML.Empleado();

                            empleadoML.IdEmpleado = empleadoObj.IdEmpleado;
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
                            empleadoML.Imagen = empleadoObj.Imagen;

                            if (empleadoObj.Imagen != null && empleadoObj.Imagen.Length > 0)
                            {
                                // Convierte el byte[] (empleadoObj.Imagen) a string Base64.
                                empleadoML.Imagen64 = Convert.ToBase64String(empleadoObj.Imagen);
                            }

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
                using (DL.GestionNominaContext context = new DL.GestionNominaContext())
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
                                                EmpleadoDb.Imagen,
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
                        empleadoML.Imagen = queryGetEmpleado.Imagen;

                        if (queryGetEmpleado.Imagen != null && queryGetEmpleado.Imagen.Length > 0)
                        {
                            // Convierte el byte[] (empleadoObj.Imagen) a string Base64.
                            empleadoML.Imagen64 = Convert.ToBase64String(queryGetEmpleado.Imagen);
                        }

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
                            @NoFaltas        = {empleado.NoFaltas},
                            @Imagen          = {empleado.Imagen}
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
                using (DL.GestionNominaContext context = new DL.GestionNominaContext())
                {
                    var queryUpdEmp = (from EmpleadoDb in context.Empleados
                                       where EmpleadoDb.IdEmpleado == empleado.IdEmpleado
                                       select EmpleadoDb).SingleOrDefault();

                    if (queryUpdEmp != null)
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
                        queryUpdEmp.Imagen = empleado.Imagen;

                        int RowsAffected = context.SaveChanges();

                        if (RowsAffected > 0)
                        {
                            resultUpdEmpleado.Correct = true;
                        }
                        else
                        {
                            resultUpdEmpleado.Correct = false;
                        }
                    }
                }

            }
            catch (Exception ex)
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


        #region MÉTODOS DE MIS APIS

        public static ML.Result GetAllAPI()
        {
            ML.Result resultEmpleado = new ML.Result();
            resultEmpleado.Objects = new List<object>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5186/api/EmpleadoAPI/");
                var responseTask = client.GetAsync("GetAll");
                responseTask.Wait(); //peticion 
                var result = responseTask.Result; //guardo mi result

                if (result.IsSuccessStatusCode)
                {
                    //var readTask = result.Content.ReadAsAsync<ML.Result>(); 
                    var readTask = result.Content.ReadFromJsonAsync<ML.Result>(); //leo json y lo convierto a result
                    readTask.Wait();

                    foreach (var resultItem in readTask.Result.Objects)
                    {
                        ML.Empleado resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Empleado>(resultItem.ToString()); //deserealiza a mi modelo de Usuario
                        resultEmpleado.Objects.Add(resultItemList);
                    }
                    resultEmpleado.Correct = true;
                }
                else
                {
                    resultEmpleado.Correct = false;
                    resultEmpleado.ErrorMessage = "No se encontraron empleados";
                }
            }
            return resultEmpleado;
        }

        public static ML.Result GetByIdAPI(int IdEmpleado)
        {
            ML.Result resultGetByID = new ML.Result();

            try
            {
                using (var client = new HttpClient())
                {
                    //client.BaseAddress = new Uri(urlAPI);

                    client.BaseAddress = new Uri("http://localhost:5186/api/EmpleadoAPI/");
                    var responseTask = client.GetAsync("GetById/" + IdEmpleado);
                    responseTask.Wait();
                    var resultAPI = responseTask.Result;

                    if (resultAPI.IsSuccessStatusCode)
                    {
                        var readTask = resultAPI.Content.ReadFromJsonAsync<ML.Result>();
                        readTask.Wait();
                        ML.Empleado resultItemList = new ML.Empleado();
                        resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Empleado>(readTask.Result.Object.ToString());
                        resultGetByID.Object = resultItemList;

                        resultGetByID.Correct = true;
                    }
                    else
                    {
                        resultGetByID.Correct = false;
                        resultGetByID.ErrorMessage = "No existen registros en la tabla Empleados";
                    }
                }
            }
            catch (Exception ex)
            {
                resultGetByID.Correct = false;
                resultGetByID.ErrorMessage = ex.Message;
            }
            return resultGetByID;
        }

        public static ML.Result AddAPI(ML.Empleado empleado)
        {
            ML.Result resultAddAPI = new ML.Result();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5186/api/EmpleadoAPI/");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<ML.Empleado>("Add", empleado); //Serializar
                postTask.Wait();
                var resultPeticionPost = postTask.Result;

                if (resultPeticionPost.IsSuccessStatusCode)
                {
                    var readTask = resultPeticionPost.Content.ReadFromJsonAsync<ML.Result>();
                    readTask.Wait();

                    resultAddAPI.Object = readTask.Result;
                    resultAddAPI.Correct = true;
                }
                else
                {
                    resultAddAPI.Correct = false;
                    resultAddAPI.ErrorMessage = "No se pudo agregar al empleado";
                }
            }
            return resultAddAPI;
        }

        public static ML.Result UpdateAPI(ML.Empleado empleado)
        {
            ML.Result resultUpdateAPI = new ML.Result();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5186/api/EmpleadoAPI/");

                //HTTP PUt
                var postTask = client.PutAsJsonAsync<ML.Empleado>("Update", empleado); //Serializar
                postTask.Wait();
                var resultPeticionPost = postTask.Result;

                if (resultPeticionPost.IsSuccessStatusCode)
                {
                    var readTask = resultPeticionPost.Content.ReadFromJsonAsync<ML.Result>();
                    readTask.Wait();

                    resultUpdateAPI.Object = readTask.Result;
                    resultUpdateAPI.Correct = true;

                }
                else
                {
                    resultUpdateAPI.Correct = false;
                    resultUpdateAPI.ErrorMessage = "No se pudo actualizar el registro.";
                }

            }
            return resultUpdateAPI;
        }

        public static ML.Result DeleteAPI(int IdEmpleado)
        {
            ML.Result resultDeleteAPI = new ML.Result();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5186/api/EmpleadoAPI/");

                //HTTP DELETE
                var postTask = client.DeleteAsync("Delete/" + IdEmpleado);
                postTask.Wait();
                var resultPeticionPost = postTask.Result;

                if (resultPeticionPost.IsSuccessStatusCode)
                {
                    var readTask = resultPeticionPost.Content.ReadFromJsonAsync<ML.Result>();
                    readTask.Wait();

                    resultDeleteAPI.Object = readTask.Result;
                    resultDeleteAPI.Correct = true;
                }
                else
                {
                    resultDeleteAPI.Correct = false;
                    resultDeleteAPI.ErrorMessage = "No se pudo eliminar el empleado";
                }
            }
            return resultDeleteAPI;
        }

        #endregion

        #region MÉTODOS DE IMAGEN

        public static ML.Empleado GetByIdImagen(int IdEmpleado)
        {
            ML.Empleado empleadoML = new ML.Empleado();

            try
            {
                using (DL.GestionNominaContext context = new DL.GestionNominaContext())
                {
                    var queryGetImagenEmpleo = (from EmpleadoDb in context.Empleados
                                            join DepartamentoDb in context.Departamentos on EmpleadoDb.IdDepartamento equals DepartamentoDb.IdDepartamento
                                            where EmpleadoDb.IdEmpleado == IdEmpleado
                                            select new
                                            {
                                                EmpleadoDb.Imagen,
                                            }).SingleOrDefault();

                    if (queryGetImagenEmpleo != null)
                    {
                        empleadoML.Imagen = queryGetImagenEmpleo.Imagen;

                        if (empleadoML != null)
                        {
                            return empleadoML;
                        }
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return empleadoML;
        }

        #endregion
    }
}
