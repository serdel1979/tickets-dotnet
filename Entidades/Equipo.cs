using System.ComponentModel.DataAnnotations;

namespace tickets.Entidades
{
    public class Equipo
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        [StringLength(450)]
        public string UsuarioId { get; set; }
        [StringLength(100)]
        public string Inventario { get; set; }
        [StringLength(100)]
        public string Serie { get; set; }
        [StringLength(250)]
        public string Comentario { get; set; }  

    }
}
