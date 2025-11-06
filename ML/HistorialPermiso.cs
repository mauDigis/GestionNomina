using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class HistorialPermiso
    { 
        public int? IdHistorialPermiso { get; set; }
        public ML.Permiso? Permiso { get; set; }
        public DateOnly? FechaRevision {  get; set; }
        public ML.StatusPermiso? StatusPermiso { get; set; }
        public string? Observaciones { get; set; }
        public ML.Empleado? Autoriza { get; set; }
        public List<object>? ListaHistorialPermisos { get; set; }
    }
}
