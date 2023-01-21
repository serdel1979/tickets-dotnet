using System.ComponentModel.DataAnnotations;

namespace tickets.DTOs
{
    public class NuevoEstadoDTO
    {
        [Required]
        [StringLength(50)]
        public string EstadoActual { get; set; }
        [Required]
        [StringLength(250)]
        public string Comentario { get; set; }
        [Required]
        public int SolicitudId { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
    }
}
