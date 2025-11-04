using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Permiso
    {
        public int? IdPermiso { get; set; }
        public Empleado? Empleado { get; set; }
        public DateOnly? FechaSolicitud { get; set; }
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }
        public TimeOnly? HoraInicio { get; set; }
        public TimeOnly? HoraFin { get; set; }
        public string? Motivo { get; set; }
        public StatusPermiso? StatusPermiso { get; set; }
        public int? IdAutorizador { get; set; }
        public Departamento? Departamento { get; set; }
        public List<object>? Permisos { get; set; }
        public ML.Empleado? Autoriza { get; set; }
    }
}
