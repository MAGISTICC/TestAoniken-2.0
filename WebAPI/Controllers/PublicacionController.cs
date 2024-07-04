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
        private readonly iPublicationService _publicacionService;

        public PublicacionController(iPublicationService publicacionService)
        {
            _publicacionService = publicacionService;
        }

        [HttpGet("pendientes")]
        public async Task<IActionResult> ObtenerPublicacionesPendientes()
        {
            var publicacionesPendientes = await _publicacionService.ObtenerPublicacionesPendientes();
            return Ok(publicacionesPendientes);
        }

        [HttpPost("aprobar")]
        public async Task<IActionResult> AprobarPublicacion([FromBody] int idPublicacion)
        {
            var resultado = await _publicacionService.AprobarPublicacion(idPublicacion);
            if (!resultado)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPost("rechazar")]
        public async Task<IActionResult> RechazarPublicacion([FromBody] int idPublicacion)
        {
            var resultado = await _publicacionService.RechazarPublicacion(idPublicacion);
            if (!resultado)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPublicacion(int id, [FromBody] Publicacion publicacionActualizada)
        {
            var resultado = await _publicacionService.ActualizarPublicacion(id, publicacionActualizada);
            if (!resultado)
            {
                return NotFound();
            }
            return Ok(publicacionActualizada);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPublicacion(int id)
        {
            var resultado = await _publicacionService.EliminarPublicacion(id);
            if (!resultado)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
