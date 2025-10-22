﻿using System;
using System.Collections.Generic;

namespace DL;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public string? Nombre { get; set; }

    public string? ApellidoPaterno { get; set; }

    public string? ApellidoMaterno { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public string? Rfc { get; set; }

    public string? Nss { get; set; }

    public string? Curp { get; set; }

    public DateOnly? FechaIngreso { get; set; }

    public int? IdDepartamento { get; set; }

    public decimal? SalarioBase { get; set; }

    public byte? NoFaltas { get; set; }

    public virtual Departamento? IdDepartamentoNavigation { get; set; }
}
