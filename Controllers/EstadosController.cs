﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using tickets.DTOs;
using tickets.Entidades;

namespace tickets.Controllers
{
    [ApiController]
    [Route("api/solicitudes/estados")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EstadosController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public EstadosController(ApplicationDbContext context,
            IMapper mapper
            )
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = "EsAdmin")]
        public async Task<ActionResult<List<EstadoDTO>>> GetEstados()
        {
            var entidadesEstado = await context.Estados.ToListAsync();
            var dtos = mapper.Map<List<EstadoDTO>>(entidadesEstado);
            return dtos;
        }
        
        
        
        
        
        
        
        [HttpGet("{id:int}", Name = "ObtenerEstado")]
        public async Task<ActionResult<EstadoDTO>> GetEstado(int id)
        {
            var entidadEstado = await context.Estados.FirstOrDefaultAsync(estado => estado.Id == id);
            if(entidadEstado == null)
            {
                return NotFound();
            }
            var dto = mapper.Map<EstadoDTO>(entidadEstado);
            return Ok(dto);
        }



        [HttpPut("{idEstado:int}")]
        [Authorize(Policy = "EsAdmin")]
        public async Task<ActionResult> Put(EstadoDTO estadoDTO, int idEstado)
        {


            var estadoBd = await context.Estados.FirstOrDefaultAsync(x => x.Id == idEstado);
            if (estadoBd == null)
            {
                return NotFound();
            }

            if(estadoBd.EstadoActual == "CERRADO" || estadoBd.EstadoActual == "SOLUCIONADO")
            {
                return Unauthorized();
            }

            estadoDTO.EstadoActual = estadoBd.EstadoActual;
            estadoDTO.Fecha = DateTime.Now;
            estadoDTO.SolicitudId = estadoBd.SolicitudId;
            estadoDTO.Id = estadoBd.Id;
            estadoBd = mapper.Map(estadoDTO, estadoBd);
            var estadoBD = mapper.Map<Estado>(estadoDTO);

            await context.SaveChangesAsync();
            return NoContent();
        }




        [HttpPost("{idSolicitud:int}/nuevo")]
        [Authorize(Policy = "EsAdmin")]
        public async Task<ActionResult> Post([FromBody] NuevoEstadoDTO nuevoEstadoDto, int idSolicitud)
        {

            nuevoEstadoDto.Fecha = DateTime.Now;
            nuevoEstadoDto.SolicitudId = idSolicitud;
            var entidadesEstado = await context.Estados.Where(estado => estado.SolicitudId == idSolicitud).
                                OrderByDescending(estado => estado.Fecha).
                                ToListAsync();
            var ultimo = entidadesEstado.OrderByDescending(x => x.Fecha).FirstOrDefault();

            if(ultimo.EstadoActual == "CERRADO" || ultimo.EstadoActual == "SOLUCIONADO")
            {
                return StatusCode(405);
            }
            if (ultimo.EstadoActual == "VISTO" || nuevoEstadoDto.EstadoActual == "PENDIENTE")
            {
                return StatusCode(405);
            }
            var entidadEstado = mapper.Map<Estado>(nuevoEstadoDto);
            context.Add(entidadEstado);
            await context.SaveChangesAsync();
            var estadoDto = mapper.Map<EstadoDTO>(entidadEstado);

            var solicitudBd = await context.Solicitudes.FirstOrDefaultAsync(x => x.Id == idSolicitud);
            solicitudBd.EstadoActual = nuevoEstadoDto.EstadoActual;
            await context.SaveChangesAsync();
            return Ok(estadoDto);
        }

    }
}
