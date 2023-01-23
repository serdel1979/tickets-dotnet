using System.ComponentModel.DataAnnotations;

namespace tickets.DTOs
{
    public class CredencialesUsuario
    {
        [Required]
        [StringLength(50)]
        public string Usuario { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
