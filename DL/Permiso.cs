using System;
using System.Collections.Generic;

namespace DL;

public partial class Permiso
{
    public int IdPermiso { get; set; }

    public int? IdEmpleado { get; set; }

    public DateOnly? FechaSolicitud { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaFin { get; set; }

    public TimeOnly? HoraInicio { get; set; }

    public TimeOnly? HoraFin { get; set; }

    public string? Motivo { get; set; }

    public byte? IdStatusPermiso { get; set; }

    public int? IdAutorizador { get; set; }

    public virtual Empleado? IdAutorizadorNavigation { get; set; }

    public virtual Empleado? IdEmpleadoNavigation { get; set; }

    public virtual StatusPermiso? IdStatusPermisoNavigation { get; set; }
}
