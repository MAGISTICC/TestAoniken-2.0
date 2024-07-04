using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TestAoniken.Models;
using TestAoniken.Servicios;

namespace TestAoniken.Servicios
{
    public class PublicacionService : iPublicationService
    {
        private readonly string _connectionString;

        public PublicacionService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Publicacion>> ObtenerPublicacionesPendientes()
        {
            var publicacionesPendientes = new List<Publicacion>();
            await using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sqlQuery = "SELECT p.Id, p.Titulo, p.Contenido, u.Id AS AutorId, u.Nombre AS NombreAutor, u.Rol AS RolAutor, p.FechaEnvio " +
                               "FROM publicaciones p " +
                               "INNER JOIN usuarios u ON p.AutorId = u.Id " +
                               "WHERE p.PendienteAprobacion = 1";
                await using (var command = new MySqlCommand(sqlQuery, connection))
                {
                    await using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
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
            return publicacionesPendientes;
        }

        public async Task<bool> AprobarPublicacion(int idPublicacion)
        {
            await using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sqlQuery = "UPDATE publicaciones SET PendienteAprobacion = 0 WHERE Id = @Id";
                await using (var command = new MySqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", idPublicacion);
                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<bool> RechazarPublicacion(int idPublicacion)
        {
            await using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sqlQuery = "DELETE FROM publicaciones WHERE Id = @Id";
                await using (var command = new MySqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", idPublicacion);
                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<bool> ActualizarPublicacion(int id, Publicacion publicacionActualizada)
        {
            await using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sqlQuery = "UPDATE publicaciones SET Titulo = @Titulo, Contenido = @Contenido WHERE Id = @Id";
                await using (var command = new MySqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Titulo", publicacionActualizada.Titulo);
                    command.Parameters.AddWithValue("@Contenido", publicacionActualizada.Contenido);
                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<bool> EliminarPublicacion(int id)
        {
            await using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sqlQuery = "DELETE FROM publicaciones WHERE Id = @Id";
                await using (var command = new MySqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
