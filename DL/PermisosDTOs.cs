using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class PermisosDTOs
    {

        //Me permite mapear los datos que obtengo de un stored procedure, para guardarlos en mis modelos.

        //Propiedades de que obtengo del stored procedure.
        public int? IdPermiso { get; set; }
        public int? IdEmpleado { get; set; }
        public DateOnly? FechaSolicitud { get; set; }
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }
        public TimeOnly? HoraInicio { get; set; }
        public TimeOnly? HoraFin { get; set; }
        public string? Motivo { get; set; }
        public byte? IdStatusPermiso { get; set; }
        public int? IdAutorizador { get; set; }
        public string? Nombre {  get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? NombreStatus { get; set; }
        public string? NombreDepartamento { get; set; }
    }
}
