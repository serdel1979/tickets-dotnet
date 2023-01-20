using System.ComponentModel.DataAnnotations;
using tickets.Entidades;

namespace tickets.DTOs
{
    public class SolicitudDTO
    {
        public int Id { get; set; }
        [Required]
        public int UsuarioId { get; set; }
        [Required]
        [StringLength(50)]
        public string Usuario { get; set; }
        [Required]
        [StringLength(250)]
        public string Descripcion { get; set; }
        [Required]
        [StringLength(250)]
        public string Equipo { get; set; }
        [Required]
        [StringLength(50)]
        public string EstadoActual { get; set; }
    }
}
