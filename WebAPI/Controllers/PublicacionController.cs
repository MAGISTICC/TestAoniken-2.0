using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using TestAoniken.Models;

namespace TestAoniken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicacionController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PublicacionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("listas")]
        public dynamic listarUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>
            {
                new Usuario
                {
                    Id = 0,
                    Nombre = "Marco",
                    Rol = "Usuario"
                }
            };

            return usuarios;
        }

        [HttpGet("pendientes")]
        public IActionResult ObtenerPublicacionesPendientes()
        {
            List<Publicacion> publicacionesPendientes = new List<Publicacion>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "SELECT p.Id, p.Titulo, p.Contenido, u.Id AS AutorId, u.Nombre AS NombreAutor, u.Rol AS RolAutor, p.FechaEnvio " +
                                  "FROM publicaciones p " +
                                  "INNER JOIN usuarios u ON p.AutorId = u.Id " +
                                  "WHERE p.PendienteAprobacion = 1";
                using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            publicacionesPendientes.Add(new Publicacion
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Titulo = reader["Titulo"].ToString(),
                                Contenido = reader["Contenido"].ToString(),
                                Autor = new Usuario
                                {
                                    Id = Convert.ToInt32(reader["AutorId"]),
                                    Nombre = reader["NombreAutor"].ToString(),
                                    Rol = reader["RolAutor"].ToString()
                                },
                                FechaEnvio = Convert.ToDateTime(reader["FechaEnvio"]),
                                PendienteAprobacion = true
                            });
                        }
                    }
                }
            }
            return Ok(publicacionesPendientes);
        }



        [HttpPost("aprobar")]
        public IActionResult AprobarPublicacion([FromBody] int idPublicacion)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "UPDATE publicaciones SET PendienteAprobacion = 0 WHERE Id = @Id";
                using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", idPublicacion);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        return NotFound();
                    }
                }
            }
            return Ok();
        }

        [HttpPost("rechazar")]
        public IActionResult RechazarPublicacion([FromBody] int idPublicacion)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "DELETE FROM publicaciones WHERE Id = @Id";
                using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", idPublicacion);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        return NotFound();
                    }
                }
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult ActualizarPublicacion(int id, [FromBody] Publicacion publicacionActualizada)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "UPDATE publicaciones SET Titulo = @Titulo, Contenido = @Contenido WHERE Id = @Id";
                using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Titulo", publicacionActualizada.Titulo);
                    command.Parameters.AddWithValue("@Contenido", publicacionActualizada.Contenido);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        return NotFound();
                    }
                }
            }
            return Ok(publicacionActualizada);
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarPublicacion(int id)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "DELETE FROM publicaciones WHERE Id = @Id";
                using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        return NotFound();
                    }
                }
            }
            return Ok();
        }
    }
}
