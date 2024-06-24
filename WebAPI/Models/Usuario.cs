using System.ComponentModel.DataAnnotations;

namespace TestAoniken.Models

{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Rol { get; set; }
    }
}
