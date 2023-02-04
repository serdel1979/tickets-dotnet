﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using tickets.DTOs;
using tickets.Entidades;
using tickets.Utilidades;

namespace tickets.Controllers
{
    [ApiController]
    [Route("api/solicitudes")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [Authorize(Policy ="EsAdmin")]
        public async Task<ActionResult<List<SolicitudDTO>>> GetSolicitudes([FromQuery] PaginacionDTO paginacion)
        {
            var queryable = context.Solicitudes.AsQueryable();
            await HttpContext.InsertaPaginacionEnCabecera(queryable);
            var entidadesSolicitud = await queryable.OrderByDescending(sol => sol.Fecha).Paginar(paginacion).
                    ToListAsync();
            return mapper.Map<List<SolicitudDTO>>(entidadesSolicitud);
        }

        [HttpGet("{idUser}/missolicitudes")]
        public async Task<ActionResult<List<SolicitudDTO>>> GetMiSolicitudes(string idUser)
        {

            var entidadesSolicitud = await context.Solicitudes.Where(x => x.UsuarioId == idUser).
                    OrderByDescending(estado => estado.Fecha).
                    ToListAsync();
            return mapper.Map<List<SolicitudDTO>>(entidadesSolicitud);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] NuevaSolicitudDTO nuevaSolicitud)
        {
   
            var entidadSolicitud = mapper.Map<Solicitud>(nuevaSolicitud);
            entidadSolicitud.EstadoActual = "PENDIENTE";
            entidadSolicitud.Fecha = DateTime.Now;
           // entidadSolicitud.UrlImagen = URL;
            context.Add(entidadSolicitud);
            await context.SaveChangesAsync();
            //Guarda el primer estado
            var estadoDto = new NuevoEstadoDTO
            {
                EstadoActual = "PENDIENTE",
                Comentario = "Nada por ahora...",
                SolicitudId = entidadSolicitud.Id,
                Fecha = DateTime.Now
            };
            var entidadEstado = mapper.Map<Estado>(estadoDto);
            context.Add(entidadEstado);
            await context.SaveChangesAsync();
            var solicitudDto = mapper.Map<SolicitudDTO>(entidadSolicitud);
            return Ok(solicitudDto);
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "EsAdmin")]
        public async Task<ActionResult<EstadoDTO>> GetSolicitud(int id)
        {
            var entidadSolicitud = await context.Solicitudes.FirstOrDefaultAsync(solicitud => solicitud.Id == id);
            if (entidadSolicitud == null)
            {
                return NotFound();
            }

            var entidadesEstado = await context.Estados.Where(estado => estado.SolicitudId == id).
                    OrderByDescending(estado => estado.Fecha).
                    ToListAsync();

            if (entidadesEstado.Count == 1)
            {    //si solo tiene un estado, es el pendiente y es un admin
                var estado = new NuevoEstadoDTO
                {
                    EstadoActual = "Visto",
                    Comentario = "Nada por ahora!!!",
                    SolicitudId = id,
                    Fecha = DateTime.Now
                };
                entidadSolicitud.EstadoActual = "Visto";
                var entidadEstado = mapper.Map<Estado>(estado);
                context.Add(entidadEstado);
                await context.SaveChangesAsync();
            };



            await context.SaveChangesAsync();
            var solicitudDto = mapper.Map<SolicitudDTO>(entidadSolicitud);
            return Ok(solicitudDto);
        }


        


        //retorna los estados de una solicitud

        [HttpGet("{idSolicitud:int}/estados", Name = "ObtenerEstadosDeSolicitud")]
        [Authorize(Policy = "EsAdmin")]
        public async Task<ActionResult<List<EstadoDTO>>> GetEstadosSolicitud(int idSolicitud)
        {
            var entidadSolicitud = await context.Solicitudes.FirstOrDefaultAsync(solicitud => solicitud.Id == idSolicitud);
            if (entidadSolicitud == null)
            {
                return NotFound();
            }

            var entidadesEstado = await context.Estados.Where(estado => estado.SolicitudId == idSolicitud).
                    OrderByDescending(estado => estado.Fecha).
                    ToListAsync();

            if (entidadesEstado.Count == 1)
            {    //si solo tiene un estado, es el pendiente y es un admin
                var estado = new NuevoEstadoDTO
                {
                    EstadoActual = "Visto",
                    Comentario = "Nada por ahora...",
                    SolicitudId = idSolicitud,
                    Fecha = DateTime.Now
                };
                entidadSolicitud.EstadoActual = "Visto";
                var entidadEstado = mapper.Map<Estado>(estado);
                context.Add(entidadEstado);
                await context.SaveChangesAsync();
            };

            entidadesEstado = await context.Estados.Where(estado => estado.SolicitudId == idSolicitud).
                    OrderByDescending(estado => estado.Fecha).
                    ToListAsync();

            if (entidadesEstado == null)
            {
                return NotFound();
            }
            var dtos = mapper.Map<List<EstadoDTO>>(entidadesEstado);
            return Ok(dtos);
        }


        [HttpGet("mi/{idSolicitud:int}/{idUser}")] //analizar si el id logueado es el idUsuario de solicitud
        public async Task<ActionResult<EstadoDTO>> GetMiSolicitud(int idSolicitud, string idUser)
        {
            var entidadSolicitud = await context.Solicitudes.FirstOrDefaultAsync(solicitud => solicitud.Id == idSolicitud);
            if (entidadSolicitud == null)
            {
                return NotFound();
            }
            if(entidadSolicitud.UsuarioId != idUser)
            {
                return StatusCode(403);
            }

            var solicitudDto = mapper.Map<SolicitudDTO>(entidadSolicitud);
            return Ok(solicitudDto);
        }

    }
}
