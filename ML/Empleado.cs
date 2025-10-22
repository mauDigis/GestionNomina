namespace ML
{
    public class Empleado
    {
        public int? IdEmpleado { get; set; }
        public string? Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public DateOnly? FechaNacimiento { get; set; }
        public string? RFC { get; set; }
        public string? NSS { get; set; }
        public string? CURP { get; set; }
        public DateOnly? FechaIngreso { get; set; }
        public Departamento? Departamento { get; set; }
        public decimal? SalarioBase { get; set; }
        public byte? NoFaltas { get; set; }
        public List<object>? Empleados { get; set; }
    }
}
