using System.ComponentModel.DataAnnotations;

namespace tickets.Entidades
{
    public class Estado
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string EstadoActual { get; set;}
        [Required]
        [StringLength(250)]
        public string Comentario { get; set; }
        [Required]
        public int SolicitudId { get; set;}
        [Required]
        public DateTime Fecha { get; set;}
    }
}
