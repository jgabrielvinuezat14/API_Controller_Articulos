using ControlInventario.Web.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace ControlInventario.Web.Api.Data;

public class BaseArticulosContext : DbContext
{
    public BaseArticulosContext(DbContextOptions<BaseArticulosContext> options) : base(options) { }

    public DbSet<Empresa> Empresas { get; set; }

    public DbSet<Familia> Familias { get; set; }

    public DbSet<SubFamilia> SubFamilias { get; set; }

    public DbSet<Articulo> Articulos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Empresa>(empresa =>
        {
            empresa.ToTable("Empresa", "dbo");
            empresa.HasKey(e => e.Id);
            empresa.Property(e => e.Id).HasDefaultValueSql("(newid())");
            empresa.Property(e => e.RazonSocial).IsRequired().IsUnicode(false);
        });

        modelBuilder.Entity<Familia>(familia =>
        {
            familia.ToTable("Familia", "Adquisiciones");
            familia.HasKey(f => new { f.EmpresaId, f.Id });
            familia.Property(f => f.Id).HasDefaultValueSql("(newid())");
            familia.Property(f => f.Nombre).HasMaxLength(70).IsRequired().IsUnicode(false);
            familia.Property(f => f.Descripcion).HasColumnType("text");

            familia.HasOne(f => f.Empresa)
                   .WithMany(e => e.Familias)
                   .HasForeignKey(f => f.EmpresaId)
                   .OnDelete(DeleteBehavior.ClientSetNull);

            familia.HasOne(f => f.FamiliaPadre)
                   .WithMany(f => f.FamiliasHijas)
                   .HasForeignKey(f => new { f.EmpresaId, f.FamiliaId });
        });

        modelBuilder.Entity<SubFamilia>(subFamilia =>
        {
            subFamilia.ToTable("SubFamilia", "Adquisiciones");
            subFamilia.HasKey(s => new { s.EmpresaId, s.Id });
            subFamilia.Property(s => s.Id).HasDefaultValueSql("(newid())");
            subFamilia.Property(s => s.Nombre).HasMaxLength(256).IsRequired().IsUnicode(false);
            subFamilia.Property(s => s.Descripcion).HasColumnType("text");

            subFamilia.HasOne(s => s.Empresa)
                      .WithMany(e => e.SubFamilias)
                      .HasForeignKey(s => s.EmpresaId)
                      .OnDelete(DeleteBehavior.ClientSetNull);

            subFamilia.HasOne(s => s.Familia)
                      .WithMany(f => f.SubFamilias)
                      .HasForeignKey(s => new { s.EmpresaId, s.FamiliaId })
                      .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Articulo>(articulo =>
        {
            articulo.ToTable("Articulo", "Adquisiciones");
            articulo.HasKey(a => new { a.EmpresaId, a.SubFamiliaId, a.Id });
            articulo.Property(a => a.Id).HasDefaultValueSql("(newid())");
            articulo.Property(a => a.Codigo).HasMaxLength(256).IsUnicode(false);
            articulo.Property(a => a.Nombre).HasMaxLength(256).IsRequired().IsUnicode(false);
            articulo.Property(a => a.Descripcion).HasColumnType("text");

            articulo.HasOne(a => a.Empresa)
                    .WithMany(e => e.Articulos)
                    .HasForeignKey(a => a.EmpresaId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            articulo.HasOne(a => a.SubFamilia)
                    .WithMany(s => s.Articulos)
                    .HasForeignKey(a => new { a.EmpresaId, a.SubFamiliaId })
                    .OnDelete(DeleteBehavior.ClientSetNull);
        });

        base.OnModelCreating(modelBuilder);
    }
}