using ControlInventario.Web.Api.Dto;

namespace ControlInventario.Web.Api.Service;

public interface IArticuloService
{
    Task<List<ArticuloRespuestaDto>> ObtenerTodosAsync();

    Task<ArticuloRespuestaDto?> ObtenerPorIdAsync(Guid id);

    Task<(ArticuloRespuestaDto? Articulo, string? Error)> CrearAsync(ArticuloDto dto);

    Task<(ArticuloRespuestaDto? Articulo, string? Error)> ActualizarAsync(Guid id, ArticuloDto dto);

    Task<bool> EliminarAsync(Guid id);
}