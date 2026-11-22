namespace ControlInventario.Web.Api.Dto;

public class ArticuloRespuestaDto
{
    public Guid Id { get; set; }

    public string? Codigo { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public bool Eliminado { get; set; }

    public string Empresa { get; set; } = string.Empty;

    public string Familia { get; set; } = string.Empty;

    public string SubFamilia { get; set; } = string.Empty;
}