using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DAL.Facturacion.Models
{
    public partial class FacturacionContext : DbContext
    {
        public FacturacionContext()
        {
        }

        public FacturacionContext(DbContextOptions<FacturacionContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Factura> Factura { get; set; }
        public virtual DbSet<FacturaDetalle> FacturaDetalle { get; set; }
        public virtual DbSet<PrecioProducto> PrecioProducto { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<TipoIdentificacion> TipoIdentificacion { get; set; }
        public virtual DbSet<PreciosFechas> PreciosFechas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Facturacion;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_cli");

                entity.ToTable("cli_Cliente");

                entity.Property(e => e.Id).HasColumnName("cli_Id");

                entity.Property(e => e.Activo)
                    .HasColumnName("cli_Activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasColumnName("cli_Apellidos")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CorreoElectronico)
                    .HasColumnName("cli_CorreoElectronico")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FechaActualizacion)
                    .HasColumnName("cli_FechaActualizacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnName("cli_FechaCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaNacimiento)
                    .HasColumnName("cli_FechaNacimiento")
                    .HasColumnType("datetime");

                entity.Property(e => e.Identificacion)
                    .IsRequired()
                    .HasColumnName("cli_Identificacion")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Nombres)
                    .IsRequired()
                    .HasColumnName("cli_Nombres")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TidId).HasColumnName("tid_Id");

                entity.HasOne(d => d.TipoIdentificacion)
                    .WithMany(p => p.ListaClientes)
                    .HasForeignKey(d => d.TidId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cli_tid");
            });

            modelBuilder.Entity<Factura>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_fac");

                entity.ToTable("fac_Factura");

                entity.Property(e => e.Id).HasColumnName("fac_Id");

                entity.Property(e => e.CliId).HasColumnName("cli_Id");

                entity.Property(e => e.Activo)
                    .HasColumnName("fac_Activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FechaActualizacion)
                    .HasColumnName("fac_FechaActualizacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnName("fac_FechaCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaExpedicion)
                    .HasColumnName("fac_FechaExpedicion")
                    .HasColumnType("datetime");

                entity.Property(e => e.Numero)
                    .IsRequired()
                    .HasColumnName("fac_Numero")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.ListaFacturas)
                    .HasForeignKey(d => d.CliId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_fac_cli");
            });

            modelBuilder.Entity<FacturaDetalle>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_fde");

                entity.ToTable("fde_FacturaDetalle");

                entity.Property(e => e.Id).HasColumnName("fde_Id");

                entity.Property(e => e.FacId).HasColumnName("fac_Id");

                entity.Property(e => e.Activo)
                    .HasColumnName("fde_Activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Cantidad).HasColumnName("fde_Cantidad");

                entity.Property(e => e.FechaActualizacion)
                    .HasColumnName("fde_FechaActualizacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnName("fde_FechaCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.PrecioUnitario)
                    .HasColumnName("fde_PrecioUnitario")
                    .HasColumnType("money");

                entity.Property(e => e.ProId).HasColumnName("pro_Id");

                entity.HasOne(d => d.Factura)
                    .WithMany(p => p.ListaFacturaDetalles)
                    .HasForeignKey(d => d.FacId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_fde_fac");

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.ListaFacturaDetalles)
                    .HasForeignKey(d => d.ProId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_fde_pro");
            });

            modelBuilder.Entity<PrecioProducto>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_ppr");

                entity.ToTable("ppr_PrecioProducto");

                entity.HasIndex(e => e.FechaInicioVigencia)
                    .HasName("IX_ppr_fechaVigencia");

                entity.Property(e => e.Id)
                    .HasColumnName("ppr_Id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Activo)
                    .HasColumnName("ppr_Activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FechaActualizacion)
                    .HasColumnName("ppr_FechaActualizacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnName("ppr_FechaCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaInicioVigencia)
                    .HasColumnName("ppr_FechaInicioVigencia")
                    .HasColumnType("datetime");

                entity.Property(e => e.PrecioUnitario)
                    .HasColumnName("ppr_PrecioUnitario")
                    .HasColumnType("money");

                entity.Property(e => e.ProId).HasColumnName("pro_Id");

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.ListaPreciosProducto)
                    .HasForeignKey(d => d.ProId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ppr_pro");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_pro");

                entity.ToTable("pro_Producto");

                entity.Property(e => e.Id).HasColumnName("pro_Id");

                entity.Property(e => e.Activo)
                    .HasColumnName("pro_Activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CantidadInventario).HasColumnName("pro_CantidadInventario");

                entity.Property(e => e.CantidadReposicion)
                    .HasColumnName("pro_CantidadReposicion")
                    .HasDefaultValueSql("((5))");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasColumnName("pro_Codigo")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FechaActualizacion)
                    .HasColumnName("pro_FechaActualizacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnName("pro_FechaCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("pro_Nombre")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoIdentificacion>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_tid");

                entity.ToTable("tid_TipoIdentificacion");

                entity.Property(e => e.Id)
                    .HasColumnName("tid_Id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Activo)
                    .HasColumnName("tid_Activo")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasColumnName("tid_Codigo")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FechaActualizacion)
                    .HasColumnName("tid_FechaActualizacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnName("tid_FechaCreacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("tid_Nombre")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PreciosFechas>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vpf_PreciosFechas");

                entity.Property(e => e.proId).HasColumnName("pro_Id");

                entity.Property(e => e.FechaFinal)
                    .HasColumnName("vpf_fechaFinal")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaInicio)
                    .HasColumnName("vpf_fechaInicio")
                    .HasColumnType("datetime");

                entity.Property(e => e.PrecioUnitario)
                    .HasColumnName("vpf_PrecioUnitario")
                    .HasColumnType("money");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
