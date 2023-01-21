using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
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
            entidadSolicitud.Fecha = DateTime.Now;
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EstadoDTO>> GetSolicitud(int id)
        {
            var entidadSolicitud = await context.Solicitudes.FirstOrDefaultAsync(solicitud => solicitud.Id == id);
            if (entidadSolicitud == null)
            {
                return NotFound();
            }
            //busco los estados de la solicitud para marcarla como vista si es que no lo está
            var entidadesEstado = await context.Estados.Where(estado => estado.SolicitudId == id).
                ToListAsync();
            if (entidadesEstado.Count == 1) {    //si solo tiene un estado, es el pendiente
                var estado = new NuevoEstadoDTO
                {
                    EstadoActual = "Visto",
                    Comentario = "Nada por ahora...",
                    SolicitudId = id,
                    Fecha = DateTime.Now
                };
                entidadSolicitud.EstadoActual = "Visto";
                var entidadEstado = mapper.Map<Estado>(estado);
                context.Add(entidadEstado);
                await context.SaveChangesAsync();
            }
            var dto = mapper.Map<SolicitudDTO>(entidadSolicitud);
            return Ok(dto);
        }





        //retorna los estados de una solicitud

        [HttpGet("{idSolicitud:int}/estados", Name = "ObtenerEstadosDeSolicitud")]
        public async Task<ActionResult<List<EstadoDTO>>> GetEstadosSolicitud(int idSolicitud)
        {
            var entidadesEstado = await context.Estados.Where(estado => estado.SolicitudId == idSolicitud).
                                OrderByDescending(estado => estado.Fecha).
                                ToListAsync();
            if (entidadesEstado == null)
            {
                return NotFound();
            }
            var dtos = mapper.Map<List<EstadoDTO>>(entidadesEstado);
            return Ok(dtos);
        }

    }
}
