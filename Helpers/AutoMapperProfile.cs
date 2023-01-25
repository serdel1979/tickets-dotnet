using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using tickets.DTOs;
using tickets.Entidades;

namespace tickets.Helpers
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Estado,EstadoDTO>().ReverseMap();
            CreateMap<NuevoEstadoDTO, Estado>();
            CreateMap<Solicitud, SolicitudDTO>().ReverseMap();
            CreateMap<NuevaSolicitudDTO, Solicitud>();
            CreateMap<IdentityUser, UsuarioDTO>();
            CreateMap<Equipo, EquipoDTO>().ReverseMap();
        }
    }
}
