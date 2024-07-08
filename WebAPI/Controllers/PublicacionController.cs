using Microsoft.AspNetCore.Mvc;
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

        // Constructor que recibe una instancia de IPublicacionService
        public PublicacionController(IPublicacionService publicacionService)
        {
            _publicacionService = publicacionService;
        }

        // Endpoint para obtener publicaciones pendientes
        [HttpGet("pendientes")]
        public async Task<IActionResult> ObtenerPublicacionesPendientes()
        {
            // Llama al método del servicio para obtener publicaciones pendientes
            var publicacionesPendientes = await _publicacionService.ObtenerPublicacionesPendientesAsync();
            return Ok(publicacionesPendientes); // Retorna las publicaciones pendientes
        }

        // Endpoint para aprobar una publicación
        [HttpPost("aprobar")]
        public async Task<IActionResult> AprobarPublicacion([FromBody] int idPublicacion)
        {
            // Llama al método del servicio para aprobar una publicación con el ID dado
            var result = await _publicacionService.AprobarPublicacionAsync(idPublicacion);
            if (!result)
            {
                return NotFound(); // Retorna 404 si la publicación no fue encontrada
            }
            return Ok(); // Retorna 200 OK si la publicación fue aprobada correctamente
        }

        // Endpoint para rechazar una publicación
        [HttpPost("rechazar")]
        public async Task<IActionResult> RechazarPublicacion([FromBody] int idPublicacion)
        {
            // Llama al método del servicio para rechazar una publicación con el ID dado
            var result = await _publicacionService.RechazarPublicacionAsync(idPublicacion);
            if (!result)
            {
                return NotFound(); // Retorna 404 si la publicación no fue encontrada
            }
            return Ok(); // Retorna 200 OK si la publicación fue rechazada correctamente
        }

        // Endpoint para actualizar una publicación
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPublicacion(int id, [FromBody] Publicacion publicacionActualizada)
        {
            // Llama al método del servicio para actualizar una publicación con el ID dado
            var result = await _publicacionService.ActualizarPublicacionAsync(id, publicacionActualizada);
            if (!result)
            {
                return NotFound(); // Retorna 404 si la publicación no fue encontrada
            }
            return Ok(publicacionActualizada); // Retorna la publicación actualizada
        }

        // Endpoint para eliminar una publicación
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPublicacion(int id)
        {
            // Llama al método del servicio para eliminar una publicación con el ID dado
            var result = await _publicacionService.EliminarPublicacionAsync(id);
            if (!result)
            {
                return NotFound(); // Retorna 404 si la publicación no fue encontrada
            }
            return Ok(); // Retorna 200 OK si la publicación fue eliminada correctamente
        }
    }
}
