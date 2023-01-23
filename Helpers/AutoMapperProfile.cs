using AutoMapper;
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

        }
    }
}
