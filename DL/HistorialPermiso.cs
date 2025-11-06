using System;
using System.Collections.Generic;

namespace DL;

public partial class HistorialPermiso
{
    public int IdHistorialPermiso { get; set; }

    public int? IdPermiso { get; set; }

    public DateOnly? FechaRevision { get; set; }

    public byte? IdStatusPermiso { get; set; }

    public string? Observaciones { get; set; }

    public int? IdAutorizador { get; set; }

    public virtual Empleado? IdAutorizadorNavigation { get; set; }

    public virtual Permiso? IdPermisoNavigation { get; set; }

    public virtual StatusPermiso? IdStatusPermisoNavigation { get; set; }
}
