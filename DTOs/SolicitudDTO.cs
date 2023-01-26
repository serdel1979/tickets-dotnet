﻿using System.ComponentModel.DataAnnotations;
using tickets.Entidades;

namespace tickets.DTOs
{
    public class SolicitudDTO
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
        public string Imagen { get; set; }
        [Required]
        [StringLength(50)]
        public string EstadoActual { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
    }
}
