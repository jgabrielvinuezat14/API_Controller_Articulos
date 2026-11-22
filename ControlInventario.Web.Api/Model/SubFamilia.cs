namespace ControlInventario.Web.Api.Model;

public class SubFamilia
{
    public Guid EmpresaId { get; set; }

    public Guid Id { get; set; }

    public int Codigo { get; set; }

    public Guid FamiliaId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public bool Eliminado { get; set; }

    public Empresa Empresa { get; set; } = null!;

    public Familia Familia { get; set; } = null!;

    public ICollection<Articulo> Articulos { get; set; } = new List<Articulo>();
}