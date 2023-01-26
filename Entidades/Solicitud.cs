using System.ComponentModel.DataAnnotations;

namespace tickets.Entidades
{
    public class Solicitud
    {
        public int Id { get; set; }
        [Required]
        [StringLength(450)]
        public string UsuarioId { get; set; }
        [Required]
        [StringLength(50)]
        public string Usuario { get; set; }
        [Required]
        [StringLength(50)]
        public string Departamento { get; set; }
        [Required]
        [StringLength(250)]
        public string Descripcion { get; set; }
        [Required]
        [StringLength(250)]
        public string Equipo { get; set; }
        [Required]
        [StringLength(50)]
        public string EstadoActual { get; set; }
        public string UrlImagen { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        public List<Estado> Estados { get; set; }
    }
}
