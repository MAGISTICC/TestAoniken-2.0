using Microsoft.AspNetCore.Mvc;
using System;
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
            try
            {
                var result = await _publicacionService.AprobarPublicacionAsync(id);
                if (result)
                {
                    return Ok("Publicación aprobada con éxito.");
                }
                return BadRequest("Error al aprobar la publicación.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        // POST: api/Publicacion/rechazar/{id}
        [HttpPost("rechazar/{id}")]
        public async Task<IActionResult> RechazarPublicacion(int id)
        {
            try
            {
                var result = await _publicacionService.RechazarPublicacionAsync(id);
                if (result)
                {
                    return Ok("Publicación rechazada con éxito.");
                }
                return BadRequest("Error al rechazar la publicación.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        // PUT: api/Publicacion/actualizar/{id}
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> ActualizarPublicacion(int id, [FromBody] Publicacion publicacionActualizada)
        {
            if (publicacionActualizada == null)
            {
                return BadRequest("Los datos de la publicación son nulos.");
            }

            try
            {
                var result = await _publicacionService.ActualizarPublicacionAsync(id, publicacionActualizada);
                if (result)
                {
                    return Ok("Publicación actualizada con éxito.");
                }
                return BadRequest("Error al actualizar la publicación.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        // DELETE: api/Publicacion/eliminar/{id}
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarPublicacion(int id)
        {
            try
            {
                var result = await _publicacionService.EliminarPublicacionAsync(id);
                if (result)
                {
                    return Ok("Publicación eliminada con éxito.");
                }
                return BadRequest("Error al eliminar la publicación.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }
    }
}
