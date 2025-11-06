using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DL;

public partial class GestionNominaContext : DbContext
{
    public GestionNominaContext()
    {
    }

    public GestionNominaContext(DbContextOptions<GestionNominaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Departamento> Departamentos { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<HistorialPermiso> HistorialPermisos { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<StatusPermiso> StatusPermisos { get; set; }

    //Definicion de DTOs
    public virtual DbSet<EmpleadosDTO> EmpleadosDTOs { get; set; }
    public virtual DbSet<PermisosDTOs> PermisosDTOs { get; set; }
    public virtual DbSet<HistorialPermisosDTO> HistorialPermisosDTOs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.; Database=GestionNomina; TrustServerCertificate=True; User ID=sa; Password=pass@word1;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Definimos que no tienen llave primaria, campos ni son tablas.

        modelBuilder.Entity<EmpleadosDTO>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<PermisosDTOs>(entity =>
        {
            entity.HasNoKey();

        });

        modelBuilder.Entity<HistorialPermisosDTO>(entity =>
        {
            entity.HasNoKey();

        });

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.IdDepartamento).HasName("PK__Departam__787A433DC51FAA9F");

            entity.ToTable("Departamento");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PK__Empleado__CE6D8B9E09291DFF");

            entity.ToTable("Empleado");

            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Curp)
                .HasMaxLength(18)
                .IsUnicode(false)
                .HasColumnName("CURP");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nss)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("NSS");
            entity.Property(e => e.Rfc)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("RFC");
            entity.Property(e => e.SalarioBase).HasColumnType("money");

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.IdDepartamento)
                .HasConstraintName("FK__Empleado__IdDepa__1273C1CD");
        });

        modelBuilder.Entity<HistorialPermiso>(entity =>
        {
            entity.HasKey(e => e.IdHistorialPermiso).HasName("PK__Historia__0000E4BF86958794");

            entity.ToTable("HistorialPermiso");

            entity.Property(e => e.Observaciones)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAutorizadorNavigation).WithMany(p => p.HistorialPermisos)
                .HasForeignKey(d => d.IdAutorizador)
                .HasConstraintName("FK__Historial__IdAut__3C69FB99");

            entity.HasOne(d => d.IdPermisoNavigation).WithMany(p => p.HistorialPermisos)
                .HasForeignKey(d => d.IdPermiso)
                .HasConstraintName("FK__Historial__IdPer__3A81B327");

            entity.HasOne(d => d.IdStatusPermisoNavigation).WithMany(p => p.HistorialPermisos)
                .HasForeignKey(d => d.IdStatusPermiso)
                .HasConstraintName("FK__Historial__IdSta__3B75D760");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermiso).HasName("PK__Permiso__0D626EC8B2BE553C");

            entity.ToTable("Permiso");

            entity.Property(e => e.FechaFin).HasColumnName("FechaFIN");
            entity.Property(e => e.Motivo)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAutorizadorNavigation).WithMany(p => p.PermisoIdAutorizadorNavigations)
                .HasForeignKey(d => d.IdAutorizador)
                .HasConstraintName("FK__Permiso__IdAutor__20C1E124");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.PermisoIdEmpleadoNavigations)
                .HasForeignKey(d => d.IdEmpleado)
                .HasConstraintName("FK__Permiso__IdEmple__1ED998B2");

            entity.HasOne(d => d.IdStatusPermisoNavigation).WithMany(p => p.Permisos)
                .HasForeignKey(d => d.IdStatusPermiso)
                .HasConstraintName("FK__Permiso__IdStatu__1FCDBCEB");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdPuesto).HasName("PK__Rol__ADAC6B9C49B3C7E0");

            entity.ToTable("Rol");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StatusPermiso>(entity =>
        {
            entity.HasKey(e => e.IdStatusPermiso).HasName("PK__StatusPe__D8526C07486EC97A");

            entity.ToTable("StatusPermiso");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
