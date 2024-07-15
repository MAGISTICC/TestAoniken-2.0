using System;
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
        private readonly IEmailService _emailService;

        public PublicacionService(AppDbContext context, IEmailService emailService)
        {
            _context = context;  // DI del DbContext
            _emailService = emailService;  // DI del EmailService
        }

        public async Task<List<Publicacion>> ObtenerPublicacionesPendientesAsync()
        {
            return await _context.Publicaciones
                .AsNoTracking()
                .Include(p => p.Autor)
                .Where(p => p.PendienteAprobacion)
                .ToListAsync();
        }

        public async Task<bool> AprobarPublicacionAsync(int idPublicacion)
        {
            try
            {
                if (idPublicacion <= 0)
                {
                    return false;
                }

                var publicacion = await _context.Publicaciones.Include(p => p.Autor).FirstOrDefaultAsync(p => p.Id == idPublicacion);  // Buscar la publicación por su ID
                if (publicacion == null)
                {
                    return false;  // Si no se encuentra la publicación, retornar falso
                }

                if (!publicacion.PendienteAprobacion)
                {
                    return false;  // Si la publicación ya está aprobada, retornar falso
                }

                publicacion.PendienteAprobacion = false;  // Cambiar el estado de pendiente a aprobado
                await _context.SaveChangesAsync();  // Guardar los cambios en la base de datos

                // Enviar correo electrónico al autor
                var autor = await _context.Usuarios.FindAsync(publicacion.AutorId);
                if (autor != null)
                {
                    var subject = "Tu publicación ha sido aprobada";
                    var message = $"Hola {autor.Nombre},\n\nTu publicación '{publicacion.Titulo}' ha sido aprobada.";
                    await _emailService.SendEmailAsync(autor.Email, subject, message);
                }

                return true;  // Retornar verdadero indicando que la operación fue exitosa
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RechazarPublicacionAsync(int idPublicacion)
        {
            try
            {
                if (idPublicacion <= 0)
                {
                    return false;
                }

                var publicacion = await _context.Publicaciones.FindAsync(idPublicacion);  // Buscar la publicación por su ID
                if (publicacion == null)
                {
                    return false;  // Si no se encuentra la publicación, retornar falso
                }

                _context.Publicaciones.Remove(publicacion);  // Eliminar la publicación de la base de datos
                await _context.SaveChangesAsync();  // Guardar los cambios en la base de datos
                return true;  // Retornar verdadero indicando que la operación fue exitosa
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ActualizarPublicacionAsync(int id, Publicacion publicacionActualizada)
        {
            try
            {
                if (id <= 0)
                {
                    return false;
                }

                if (publicacionActualizada == null)
                {
                    return false;
                }

                if (string.IsNullOrWhiteSpace(publicacionActualizada.Titulo) || string.IsNullOrWhiteSpace(publicacionActualizada.Contenido))
                {
                    return false;
                }

                var publicacion = await _context.Publicaciones.FindAsync(id);  // Buscar la publicación por su ID
                if (publicacion == null)
                {
                    return false;  // Si no se encuentra la publicación, retornar falso
                }

                publicacion.Titulo = publicacionActualizada.Titulo;  // Actualizar el título de la publicación
                publicacion.Contenido = publicacionActualizada.Contenido;  // Actualizar el contenido de la publicación
                await _context.SaveChangesAsync();  // Guardar los cambios en la base de datos
                return true;  // Retornar verdadero indicando que la operación fue exitosa
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EliminarPublicacionAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return false;
                }

                var publicacion = await _context.Publicaciones.FindAsync(id);  // Buscar la publicación por su ID
                if (publicacion == null)
                {
                    return false;  // Si no se encuentra la publicación, retornar falso
                }

                _context.Publicaciones.Remove(publicacion);  // Eliminar la publicación de la base de datos
                await _context.SaveChangesAsync();  // Guardar los cambios en la base de datos
                return true;  // Retornar verdadero indicando que la operación fue exitosa
            }
            catch
            {
                return false;
            }
        }
    }
}
