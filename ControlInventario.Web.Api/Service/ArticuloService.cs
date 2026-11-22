using ControlInventario.Web.Api.Data;
using ControlInventario.Web.Api.Dto;
using ControlInventario.Web.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace ControlInventario.Web.Api.Service;

public class ArticuloService : IArticuloService
{
    private readonly BaseArticulosContext _contexto;
    private readonly ILogger<ArticuloService> _logger;

    public ArticuloService(BaseArticulosContext contexto, ILogger<ArticuloService> logger) {
        _contexto = contexto;
        _logger = logger;
    }

    public Task<List<ArticuloRespuestaDto>> ObtenerTodosAsync()
        => Proyectar(_contexto.Articulos.AsNoTracking().OrderBy(a => a.Nombre))
            .ToListAsync();

    public Task<ArticuloRespuestaDto?> ObtenerPorIdAsync(Guid id)
        => Proyectar(_contexto.Articulos.AsNoTracking().Where(a => a.Id == id))
            .FirstOrDefaultAsync();

    public async Task<(ArticuloRespuestaDto? Articulo, string? Error)> CrearAsync(ArticuloDto dto) {
        var error = await ValidarRelacionesAsync(dto.EmpresaId, dto.SubFamiliaId);
        if (error is not null)
        {
            return (null, error);
        }

        var articulo = new Articulo
        {
            Id = Guid.NewGuid(),
            EmpresaId = dto.EmpresaId,
            SubFamiliaId = dto.SubFamiliaId,
            Codigo = dto.Codigo?.Trim(),
            Nombre = dto.Nombre.Trim(),
            Descripcion = dto.Descripcion?.Trim(),
            Eliminado = dto.Eliminado
        };

        _contexto.Articulos.Add(articulo);
        await _contexto.SaveChangesAsync();

        _logger.LogInformation("Artículo creado con Id {ArticuloId}", articulo.Id);

        return (await ObtenerPorIdAsync(articulo.Id), null);
    }

    public async Task<(ArticuloRespuestaDto? Articulo, string? Error)> ActualizarAsync(Guid id, ArticuloDto dto) {
        var articulo = await _contexto.Articulos.FirstOrDefaultAsync(a => a.Id == id);
        if (articulo is null)
        {
            return (null, null);
        }

        var error = await ValidarRelacionesAsync(dto.EmpresaId, dto.SubFamiliaId);
        if (error is not null)
        {
            return (null, error);
        }

        var cambiaClaveCompuesta = articulo.EmpresaId != dto.EmpresaId || articulo.SubFamiliaId != dto.SubFamiliaId;

        if (cambiaClaveCompuesta)
        {
            var actualizado = new Articulo
            {
                Id = articulo.Id,
                EmpresaId = dto.EmpresaId,
                SubFamiliaId = dto.SubFamiliaId,
                Codigo = dto.Codigo?.Trim(),
                Nombre = dto.Nombre.Trim(),
                Descripcion = dto.Descripcion?.Trim(),
                Eliminado = dto.Eliminado
            };

            _contexto.Articulos.Remove(articulo);
            _contexto.Articulos.Add(actualizado);
        }
        else
        {
            articulo.Codigo = dto.Codigo?.Trim();
            articulo.Nombre = dto.Nombre.Trim();
            articulo.Descripcion = dto.Descripcion?.Trim();
            articulo.Eliminado = dto.Eliminado;
        }

        await _contexto.SaveChangesAsync();

        _logger.LogInformation("Artículo actualizado con Id {ArticuloId}", id);

        return (await ObtenerPorIdAsync(id), null);
    }

    public async Task<bool> EliminarAsync(Guid id) {
        var articulo = await _contexto.Articulos.FirstOrDefaultAsync(a => a.Id == id);
        if (articulo is null)
        {
            _logger.LogWarning("Intento de eliminar artículo inexistente con Id {ArticuloId}", id);
            return false;
        }

        _contexto.Articulos.Remove(articulo);
        await _contexto.SaveChangesAsync();

        _logger.LogInformation("Artículo eliminado con Id {ArticuloId}", id);
        return true;
    }

    private async Task<string?> ValidarRelacionesAsync(Guid empresaId, Guid subFamiliaId) {
        if (empresaId == Guid.Empty)
        {
            return "El EmpresaId no puede estar vacío.";
        }

        if (subFamiliaId == Guid.Empty)
        {
            return "El SubFamiliaId no puede estar vacío.";
        }

        if (!await _contexto.Empresas.AnyAsync(e => e.Id == empresaId))
        {
            return $"No existe una empresa con Id {empresaId}.";
        }

        var existeSubFamilia = await _contexto.SubFamilias
            .AnyAsync(s => s.Id == subFamiliaId && s.EmpresaId == empresaId);

        if (!existeSubFamilia)
        {
            return $"No existe la subfamilia {subFamiliaId} para la empresa {empresaId}.";
        }

        return null;
    }

    private static IQueryable<ArticuloRespuestaDto> Proyectar(IQueryable<Articulo> consulta)
        => consulta.Select(a => new ArticuloRespuestaDto
        {
            Id = a.Id,
            Codigo = a.Codigo,
            Nombre = a.Nombre,
            Descripcion = a.Descripcion,
            Eliminado = a.Eliminado,
            Empresa = a.Empresa.RazonSocial,
            Familia = a.SubFamilia.Familia.Nombre,
            SubFamilia = a.SubFamilia.Nombre
        });
}