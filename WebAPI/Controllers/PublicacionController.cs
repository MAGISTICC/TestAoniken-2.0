using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestAoniken.Models;
using TestAoniken.Servicios;

namespace TestAoniken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicacionController : ControllerBase
    {
        private readonly IPublicacionService _publicacionService;

        public PublicacionController(IPublicacionService publicacionService)
        {
            _publicacionService = publicacionService;
        }

        // GET: api/Publicacion/pendientes
        [HttpGet("pendientes")]
        public async Task<IActionResult> ObtenerPublicacionesPendientes()
        {
            var publicacionesPendientes = await _publicacionService.ObtenerPublicacionesPendientesAsync();
            if (publicacionesPendientes == null || publicacionesPendientes.Count == 0)
            {
                return NotFound("No hay publicaciones pendientes.");
            }
            return Ok(publicacionesPendientes);
        }

        // POST: api/Publicacion/aprobar/{id}
        [HttpPost("aprobar/{id}")]
        public async Task<IActionResult> AprobarPublicacion(int id)
        {
            var result = await _publicacionService.AprobarPublicacionAsync(id);
            if (result)
            {
                return Ok("Publicación aprobada con éxito.");
            }
            return BadRequest("Error al aprobar la publicación.");
        }

        // POST: api/Publicacion/rechazar/{id}
        [HttpPost("rechazar/{id}")]
        public async Task<IActionResult> RechazarPublicacion(int id)
        {
            var result = await _publicacionService.RechazarPublicacionAsync(id);
            if (result)
            {
                return Ok("Publicación rechazada con éxito.");
            }
            return BadRequest("Error al rechazar la publicación.");
        }

        // PUT: api/Publicacion/actualizar/{id}
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> ActualizarPublicacion(int id, [FromBody] Publicacion publicacionActualizada)
        {
            if (publicacionActualizada == null)
            {
                return BadRequest("Los datos de la publicación son nulos.");
            }

            var result = await _publicacionService.ActualizarPublicacionAsync(id, publicacionActualizada);
            if (result)
            {
                return Ok("Publicación actualizada con éxito.");
            }
            return BadRequest("Error al actualizar la publicación.");
        }

        // DELETE: api/Publicacion/eliminar/{id}
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarPublicacion(int id)
        {
            var result = await _publicacionService.EliminarPublicacionAsync(id);
            if (result)
            {
                return Ok("Publicación eliminada con éxito.");
            }
            return BadRequest("Error al eliminar la publicación.");
        }
    }
}
