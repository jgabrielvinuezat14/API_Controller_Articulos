namespace ControlInventario.Web.Api.Model;

public class Familia
{
    public Guid EmpresaId { get; set; }

    public Guid Id { get; set; }

    public Guid? FamiliaId { get; set; }

    public int Codigo { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public bool Eliminado { get; set; }

    public Empresa Empresa { get; set; } = null!;

    public Familia? FamiliaPadre { get; set; }

    public ICollection<Familia> FamiliasHijas { get; set; } = new List<Familia>();

    public ICollection<SubFamilia> SubFamilias { get; set; } = new List<SubFamilia>();
}