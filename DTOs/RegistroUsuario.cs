using System.ComponentModel.DataAnnotations;

namespace tickets.DTOs
{
    public class RegistroUsuario
    {
        [Required]
        [StringLength(50)]
        public string Usuario { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set;}
    }
}
