using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class HistorialPermisosDTO
    {
        public int? IdHistorialPermiso { get; set; }
        public int? IdPermiso { get; set; }
        public DateOnly? FechaRevision { get; set; }
        public byte? IdStatusPermiso { get; set; } 
        public string? Observaciones { get; set; }
        public int? IdAutorizador { get; set; }
        public string? NombreStatus { get; set; }
        public string? NombreAutorizador { get; set; }
        public string? ApellidoPatAutorizador { get; set; }
        public string? ApellidoMatAutorizador { get; set; }
    }
}
