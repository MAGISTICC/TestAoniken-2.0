using System.Collections.Generic;
using System.Threading.Tasks;
using TestAoniken.Models;

namespace TestAoniken.Servicios
{
    public interface iPublicationService
    {
        Task<List<Publicacion>> ObtenerPublicacionesPendientes();
        Task<bool> AprobarPublicacion(int idPublicacion);
        Task<bool> RechazarPublicacion(int idPublicacion);
        Task<bool> ActualizarPublicacion(int id, Publicacion publicacionActualizada);
        Task<bool> EliminarPublicacion(int id);
    }
}
