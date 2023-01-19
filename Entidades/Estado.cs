using System.ComponentModel.DataAnnotations;

namespace tickets.Entidades
{
    public class Estado
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Descripcion { get; set;}
        public int IdSolicitud { get; set;}
        public DateTime Fecha { get; set;}
    }
}
