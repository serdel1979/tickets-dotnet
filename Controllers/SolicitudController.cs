using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tickets.DTOs;
using tickets.Entidades;

namespace tickets.Controllers
{
    [ApiController]
    [Route("api/solicitudes")]
    public class SolicitudController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public SolicitudController(ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }



        [HttpGet]
        public async Task<ActionResult<List<SolicitudDTO>>> GetSolicitudes()
        {
            var entidadesSolicitud = await context.Solicitudes.ToListAsync();
            var dtos = mapper.Map<List<SolicitudDTO>>(entidadesSolicitud);
            return dtos;
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] NuevaSolicitudDTO nuevaSolicitud)
        {
            var entidadSolicitud = mapper.Map<Solicitud>(nuevaSolicitud);
            entidadSolicitud.EstadoActual = "PENDIENTE";
            context.Add(entidadSolicitud);
            await context.SaveChangesAsync();
            //Guarda el primer estado
            var estado = new NuevoEstadoDTO
            {
                EstadoActual = "PENDIENTE",
                Comentario = "Nada por ahora...",
                SolicitudId = entidadSolicitud.Id,
                Fecha = DateTime.Now
            };
            var entidadEstado = mapper.Map<Estado>(estado);
            context.Add(entidadEstado);
            await context.SaveChangesAsync();
            var solicitudDto = mapper.Map<SolicitudDTO>(entidadSolicitud);
            return Ok(solicitudDto);
        }


    }
}
