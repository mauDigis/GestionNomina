using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class EmpleadosDTO
    {

        //Me permite mapear los datos que obtengo de un stored procedure, para guardarlos en mis modelos.

        //Propiedades de que obtengo del stored procedure.
        public int? IdEmpleado { get; set; }
        public string? Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public DateOnly? FechaNacimiento { get; set; }
        public string? RFC { get; set; }
        public string? NSS { get; set; }
        public string? CURP { get; set; }
        public DateOnly? FechaIngreso { get; set; }
        public int IdDepartamento { get; set; }
        public decimal? SalarioBase { get; set; }
        public byte? NoFaltas { get; set; }
        public string? Descripcion { get; set; }
    }
}
