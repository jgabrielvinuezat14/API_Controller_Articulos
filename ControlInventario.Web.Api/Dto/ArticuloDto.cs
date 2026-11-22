using System.ComponentModel.DataAnnotations;

namespace ControlInventario.Web.Api.Dto;

public class ArticuloDto
{
    [Required(ErrorMessage = "El EmpresaId es obligatorio.")]
    public Guid EmpresaId { get; set; }

    [Required(ErrorMessage = "El SubFamiliaId es obligatorio.")]
    public Guid SubFamiliaId { get; set; }

    [StringLength(256, ErrorMessage = "El código no puede superar los 256 caracteres.")]
    public string? Codigo { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(256, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 256 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public bool Eliminado { get; set; }
}
