using System;
using System.Collections.Generic;

namespace DL;

public partial class StatusPermiso
{
    public byte IdStatusPermiso { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<HistorialPermiso> HistorialPermisos { get; set; } = new List<HistorialPermiso>();

    public virtual ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();
}
