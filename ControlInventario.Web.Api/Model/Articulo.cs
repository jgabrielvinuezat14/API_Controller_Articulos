namespace ControlInventario.Web.Api.Model;

public class Articulo
{
    public Guid EmpresaId { get; set; }

    public Guid SubFamiliaId { get; set; }

    public Guid Id { get; set; }

    public string? Codigo { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public bool Eliminado { get; set; }

    public Empresa Empresa { get; set; } = null!;

    public SubFamilia SubFamilia { get; set; } = null!;
}