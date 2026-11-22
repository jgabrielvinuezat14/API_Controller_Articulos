namespace ControlInventario.Web.Api.Model;

public class Empresa
{
    public Guid Id { get; set; }

    public string RazonSocial { get; set; } = string.Empty;

    public ICollection<Familia> Familias { get; set; } = new List<Familia>();

    public ICollection<SubFamilia> SubFamilias { get; set; } = new List<SubFamilia>();

    public ICollection<Articulo> Articulos { get; set; } = new List<Articulo>();
}