using ControlInventario.Web.Api.Dto;
using ControlInventario.Web.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControlInventario.Web.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class ArticulosController : ControllerBase
{
    private readonly IArticuloService _servicio;

    public ArticulosController(IArticuloService servicio) => _servicio = servicio;

    [HttpGet]
    [ProducesResponseType(typeof(List<ArticuloRespuestaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObtenerTodos()
        => Ok(await _servicio.ObtenerTodosAsync());

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ArticuloRespuestaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObtenerPorId(Guid id) {
        var articulo = await _servicio.ObtenerPorIdAsync(id);
        return articulo is not null ? Ok(articulo) : NotFound();
    }

    [HttpPost]
    [ProducesResponseType(typeof(ArticuloRespuestaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Crear(ArticuloDto dto) {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var (articulo, error) = await _servicio.CrearAsync(dto);
        if (error is not null)
        {
            ModelState.AddModelError(string.Empty, error);
            return BadRequest(ModelState);
        }

        return CreatedAtAction(nameof(ObtenerPorId), new { id = articulo!.Id }, articulo);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ArticuloRespuestaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Actualizar(Guid id, ArticuloDto dto) {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var (articulo, error) = await _servicio.ActualizarAsync(id, dto);
        if (error is not null)
        {
            ModelState.AddModelError(string.Empty, error);
            return BadRequest(ModelState);
        }

        return articulo is not null ? Ok(articulo) : NotFound();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Eliminar(Guid id)
        => await _servicio.EliminarAsync(id) ? NoContent() : NotFound();
}
