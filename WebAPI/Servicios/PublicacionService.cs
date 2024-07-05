using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestAoniken.Data;
using TestAoniken.Models;

namespace TestAoniken.Servicios
{
    public class PublicacionService : IPublicacionService
    {
        private readonly AppDbContext _context;

        public PublicacionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Publicacion>> ObtenerPublicacionesPendientesAsync()
        {
            return await _context.Publicaciones
                .Include(p => p.Autor)
                .Where(p => p.PendienteAprobacion)
                .ToListAsync();
        }

        public async Task<bool> AprobarPublicacionAsync(int idPublicacion)
        {
            var publicacion = await _context.Publicaciones.FindAsync(idPublicacion);
            if (publicacion == null)
            {
                return false;
            }

            publicacion.PendienteAprobacion = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RechazarPublicacionAsync(int idPublicacion)
        {
            var publicacion = await _context.Publicaciones.FindAsync(idPublicacion);
            if (publicacion == null)
            {
                return false;
            }

            _context.Publicaciones.Remove(publicacion);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarPublicacionAsync(int id, Publicacion publicacionActualizada)
        {
            var publicacion = await _context.Publicaciones.FindAsync(id);
            if (publicacion == null)
            {
                return false;
            }

            publicacion.Titulo = publicacionActualizada.Titulo;
            publicacion.Contenido = publicacionActualizada.Contenido;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarPublicacionAsync(int id)
        {
            var publicacion = await _context.Publicaciones.FindAsync(id);
            if (publicacion == null)
            {
                return false;
            }

            _context.Publicaciones.Remove(publicacion);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
