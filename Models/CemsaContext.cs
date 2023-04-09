using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Cemsa_BackEnd.Models
{
    public partial class CemsaContext : DbContext
    {
        public CemsaContext()
        {
        }

        public CemsaContext(DbContextOptions<CemsaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TCentral> TCentrals { get; set; } = null!;
        public virtual DbSet<TCliente> TClientes { get; set; } = null!;
        public virtual DbSet<TEstadoCentral> TEstadoCentrals { get; set; } = null!;
        public virtual DbSet<TEstadoserviciosxCentral> TEstadoserviciosxCentrals { get; set; } = null!;
        public virtual DbSet<TFumigacion> TFumigacions { get; set; } = null!;
        public virtual DbSet<TServicio> TServicios { get; set; } = null!;
        public virtual DbSet<TServiciosxcentral> TServiciosxcentrals { get; set; } = null!;
        public virtual DbSet<TTipoDocumento> TTipoDocumentos { get; set; } = null!;
        public virtual DbSet<TUsuario> TUsuarios { get; set; } = null!;
        public virtual DbSet<Tmedicion> Tmedicions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseMySql("server=cemsa2022.cglkscvatwah.us-east-1.rds.amazonaws.com;database=Cemsa;user=administrador;password=Cemsa2022$", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<TCentral>(entity =>
            {
                entity.HasKey(e => e.CenNro)
                    .HasName("PRIMARY");

                entity.ToTable("tCentral");

                entity.HasIndex(e => new { e.CenTipoDoc, e.CenNroDoc }, "cen_tipoNroDoc_FK_idx");

                entity.HasIndex(e => e.CenIdEstadoCentral, "tcentral_idEstado_fk1");

                entity.Property(e => e.CenNro).HasColumnName("cen_nro");

                entity.Property(e => e.CenCoorX)
                    .HasMaxLength(50)
                    .HasColumnName("cen_coorX");

                entity.Property(e => e.CenCoorY)
                    .HasMaxLength(50)
                    .HasColumnName("cen_coorY");

                entity.Property(e => e.CenFechaAlta)
                    .HasColumnType("datetime")
                    .HasColumnName("cen_fechaAlta")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CenFechaBaja)
                    .HasColumnType("datetime")
                    .HasColumnName("cen_fechaBaja");

                entity.Property(e => e.CenIdEstadoCentral).HasColumnName("cen_idEstadoCentral");

                entity.Property(e => e.CenImei)
                    .HasMaxLength(17)
                    .HasColumnName("cen_IMEI");

                entity.Property(e => e.CenNroDoc)
                    .HasMaxLength(11)
                    .HasColumnName("cen_nroDoc");

                entity.Property(e => e.CenTipoDoc).HasColumnName("cen_tipoDoc");

                entity.HasOne(d => d.CenIdEstadoCentralNavigation)
                    .WithMany(p => p.TCentrals)
                    .HasForeignKey(d => d.CenIdEstadoCentral)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cen_idEstadoCentral_FK");

                entity.HasOne(d => d.Cen)
                    .WithMany(p => p.TCentrals)
                    .HasForeignKey(d => new { d.CenTipoDoc, d.CenNroDoc })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cen_tipoNroDoc_FK");
            });

            modelBuilder.Entity<TCliente>(entity =>
            {
                entity.HasKey(e => new { e.CliTipoDoc, e.CliNroDoc })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("tCliente");

                entity.HasIndex(e => e.CliIdUsuario, "usr_idRol_FK_idx");

                entity.HasIndex(e => e.CliNroDoc, "usr_nroDoc_idx");

                entity.HasIndex(e => e.CliTipoDoc, "usr_tipoDoc_FK_idx");

                entity.Property(e => e.CliTipoDoc).HasColumnName("cli_tipoDoc");

                entity.Property(e => e.CliNroDoc)
                    .HasMaxLength(11)
                    .HasColumnName("cli_nroDoc");

                entity.Property(e => e.CliApeNomDen)
                    .HasMaxLength(50)
                    .HasColumnName("cli_apeNomDen");

                entity.Property(e => e.CliEmail)
                    .HasMaxLength(30)
                    .HasColumnName("cli_email");

                entity.Property(e => e.CliFechaAlta)
                    .HasColumnType("datetime")
                    .HasColumnName("cli_fechaAlta")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CliIdUsuario).HasColumnName("cli_idUsuario");

                entity.Property(e => e.CliTelefono)
                    .HasMaxLength(20)
                    .HasColumnName("cli_telefono");

                entity.Property(e => e.FechaBaja).HasColumnType("datetime");

                entity.HasOne(d => d.CliIdUsuarioNavigation)
                    .WithMany(p => p.TClientes)
                    .HasForeignKey(d => d.CliIdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tCliente_FK");

                entity.HasOne(d => d.CliTipoDocNavigation)
                    .WithMany(p => p.TClientes)
                    .HasForeignKey(d => d.CliTipoDoc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("usr_tipoDoc_FK");
            });

            modelBuilder.Entity<TEstadoCentral>(entity =>
            {
                entity.HasKey(e => e.EstId)
                    .HasName("PRIMARY");

                entity.ToTable("tEstadoCentral");

                entity.Property(e => e.EstId)
                    .ValueGeneratedNever()
                    .HasColumnName("est_id");

                entity.Property(e => e.EstDescripcion)
                    .HasMaxLength(100)
                    .HasColumnName("est_descripcion");
            });

            modelBuilder.Entity<TEstadoserviciosxCentral>(entity =>
            {
                entity.HasKey(e => e.EstId)
                    .HasName("PRIMARY");

                entity.ToTable("tEstadoserviciosxCentral");

                entity.Property(e => e.EstId)
                    .ValueGeneratedNever()
                    .HasColumnName("est_id");

                entity.Property(e => e.EstDescripcion)
                    .HasMaxLength(100)
                    .HasColumnName("est_descripcion");
            });

            modelBuilder.Entity<TFumigacion>(entity =>
            {
                entity.HasKey(e => e.FumId)
                    .HasName("PRIMARY");

                entity.ToTable("tFumigacion");

                entity.HasIndex(e => e.FumNroCentral, "fum_nroCentral_FK_idx");

                entity.Property(e => e.FumId).HasColumnName("fum_id");

                entity.Property(e => e.FumFechaAlta)
                    .HasColumnType("datetime")
                    .HasColumnName("fum_fechaAlta")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.FumFechaRealizacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fum_fechaRealizacion");

                entity.Property(e => e.FumNroCentral).HasColumnName("fum_nroCentral");

                entity.Property(e => e.FumObservacion)
                    .HasMaxLength(50)
                    .HasColumnName("fum_observacion");

                entity.HasOne(d => d.FumNroCentralNavigation)
                    .WithMany(p => p.TFumigacions)
                    .HasForeignKey(d => d.FumNroCentral)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fum_nroCentral_FK");
            });

            modelBuilder.Entity<TServicio>(entity =>
            {
                entity.HasKey(e => e.SerId)
                    .HasName("PRIMARY");

                entity.ToTable("tServicio");

                entity.Property(e => e.SerId).HasColumnName("ser_id");

                entity.Property(e => e.SerDescripcion)
                    .HasMaxLength(20)
                    .HasColumnName("ser_descripcion");

                entity.Property(e => e.SerUnidad)
                    .HasMaxLength(12)
                    .HasColumnName("ser_unidad");
            });

            modelBuilder.Entity<TServiciosxcentral>(entity =>
            {
                entity.HasKey(e => new { e.SxcNroCentral, e.SxcNroServicio })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("tServiciosxcentral");

                entity.HasIndex(e => e.SxcEstado, "sxc_estado_FK_idx");

                entity.HasIndex(e => e.SxcNroServicio, "tserviciosXCentral_Servicio_fk_idx");

                entity.Property(e => e.SxcNroCentral).HasColumnName("sxc_nroCentral");

                entity.Property(e => e.SxcNroServicio).HasColumnName("sxc_nroServicio");

                entity.Property(e => e.SxcEstado).HasColumnName("sxc_estado");

                entity.Property(e => e.SxcFechaAlta)
                    .HasColumnType("datetime")
                    .HasColumnName("sxc_fecha_alta")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.SxcFechaBaja).HasColumnName("sxc_fecha_baja");

                entity.HasOne(d => d.SxcEstadoNavigation)
                    .WithMany(p => p.TServiciosxcentrals)
                    .HasForeignKey(d => d.SxcEstado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sxc_estado_FK");

                entity.HasOne(d => d.SxcNroCentralNavigation)
                    .WithMany(p => p.TServiciosxcentrals)
                    .HasForeignKey(d => d.SxcNroCentral)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sxc_nroCentral_FK");

                entity.HasOne(d => d.SxcNroServicioNavigation)
                    .WithMany(p => p.TServiciosxcentrals)
                    .HasForeignKey(d => d.SxcNroServicio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sxc_nroServicio_FK");
            });

            modelBuilder.Entity<TTipoDocumento>(entity =>
            {
                entity.HasKey(e => e.TdId)
                    .HasName("PRIMARY");

                entity.ToTable("tTipoDocumento");

                entity.Property(e => e.TdId).HasColumnName("td_id");

                entity.Property(e => e.TdDescripcion)
                    .HasMaxLength(100)
                    .HasColumnName("td_descripcion");
            });

            modelBuilder.Entity<TUsuario>(entity =>
            {
                entity.HasKey(e => e.UsrId)
                    .HasName("PRIMARY");

                entity.ToTable("tUsuario");

                entity.Property(e => e.UsrId).HasColumnName("usr_id");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.Usuario)
                    .HasMaxLength(50)
                    .HasColumnName("usuario");
            });

            modelBuilder.Entity<Tmedicion>(entity =>
            {
                entity.HasKey(e => new { e.MedId, e.MedNro, e.MedSer })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("tmedicion");

                entity.HasIndex(e => new { e.MedNro, e.MedSer }, "med_serviciosxCentral_FK");

                entity.Property(e => e.MedId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("med_id");

                entity.Property(e => e.MedNro).HasColumnName("med_nro");

                entity.Property(e => e.MedSer).HasColumnName("med_ser");

                entity.Property(e => e.MedFechaHoraBd)
                    .HasColumnType("datetime")
                    .HasColumnName("med_fechaHoraBD")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.MedFechaHoraSms)
                    .HasColumnType("datetime")
                    .HasColumnName("med_fechaHoraSMS");

                entity.Property(e => e.MedObservacion)
                    .HasMaxLength(100)
                    .HasColumnName("med_observacion");

                entity.Property(e => e.MedValor)
                    .HasPrecision(6, 2)
                    .HasColumnName("med_valor");

                entity.HasOne(d => d.Med)
                    .WithMany(p => p.Tmedicions)
                    .HasForeignKey(d => new { d.MedNro, d.MedSer })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("med_serviciosxCentral_FK");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
