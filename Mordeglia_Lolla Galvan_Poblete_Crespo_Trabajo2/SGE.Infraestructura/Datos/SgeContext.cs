using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Dominio.Enums;
using SGE.Dominio.Usuarios;
using System.Collections.Generic;

namespace SGE.Infraestructura.Datos;

public class SgeContext : DbContext
{
    public SgeContext(DbContextOptions<SgeContext> options) : base(options)
    {
    }

    public DbSet<Expediente> Expedientes { get; set; } = null!;
    public DbSet<Tramite> Tramites { get; set; } = null!;
    public DbSet<Usuario> Usuarios { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Expediente>(builder =>
        {
            builder.ToTable("Expedientes");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            
            builder.ComplexProperty(e => e.Caratula, caratula =>
            {
                caratula.Property(valor => valor.Texto)
                    .HasColumnName("Caratula")
                    .IsRequired();
            });
        });

        modelBuilder.Entity<Tramite>(builder =>
        {
            builder.ToTable("Tramites");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever();

            builder.ComplexProperty(t => t.Contenido, contenido =>
            {
                contenido.Property(valor => valor.Texto)
                    .HasColumnName("Contenido")
                    .IsRequired();
            });

            builder.HasOne<Expediente>()
                .WithMany()
                .HasForeignKey(t => t.ExpedienteId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Usuario>(builder =>
        {
            builder.ToTable("Usuarios");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedNever();
            builder.HasIndex(u => u.CorreoElectronico).IsUnique();

            builder.Property<HashSet<Permiso>>("_permisos")
                .HasColumnName("Permisos")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<HashSet<Permiso>>(v, (JsonSerializerOptions?)null) ?? new HashSet<Permiso>()
                )
                .Metadata.SetValueComparer(new ValueComparer<HashSet<Permiso>>(
                    (left, right) => left!.SetEquals(right!),
                    value => value.Aggregate(0, (hash, permiso) => HashCode.Combine(hash, permiso)),
                    value => value.ToHashSet()));
        });
    }
}
