﻿using System.ComponentModel.DataAnnotations;
using tickets.Entidades;

namespace tickets.DTOs
{
    public class NuevaSolicitudDTO
    {
        [Required]
        public int UsuarioId { get; set; }
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

    }
}
