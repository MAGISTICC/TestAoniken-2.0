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

        [HttpGet("pendientes")]
        public async Task<IActionResult> ObtenerPublicacionesPendientes()
        {
            var publicacionesPendientes = await _publicacionService.ObtenerPublicacionesPendientesAsync();
            return Ok(publicacionesPendientes);
        }

        [HttpPost("aprobar")]
        public async Task<IActionResult> AprobarPublicacion([FromBody] int idPublicacion)
        {
            var result = await _publicacionService.AprobarPublicacionAsync(idPublicacion);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPost("rechazar")]
        public async Task<IActionResult> RechazarPublicacion([FromBody] int idPublicacion)
        {
            var result = await _publicacionService.RechazarPublicacionAsync(idPublicacion);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPublicacion(int id, [FromBody] Publicacion publicacionActualizada)
        {
            var result = await _publicacionService.ActualizarPublicacionAsync(id, publicacionActualizada);
            if (!result)
            {
                return NotFound();
            }
            return Ok(publicacionActualizada);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPublicacion(int id)
        {
            var result = await _publicacionService.EliminarPublicacionAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
