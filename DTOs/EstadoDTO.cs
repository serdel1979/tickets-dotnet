using System.ComponentModel.DataAnnotations;

namespace tickets.DTOs
{
    public class EstadoDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string EstadoActual { get; set; }
        [Required]
        [StringLength(250)]
        public string Comentario { get; set; }
        public int SolicitudId { get; set; }
        public DateTime Fecha { get; set; }
    }
}
