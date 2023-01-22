using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tickets.DTOs;
using tickets.Entidades;

namespace tickets.Controllers
{
    [ApiController]
    [Route("api/solicitudes/estados")]
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


       



        [HttpPost("{id:int}/nuevo")]
        public async Task<ActionResult> Post([FromBody] NuevoEstadoDTO nuevoEstadoDto, int Id)
        {

            var entidadesEstado = await context.Estados.Where(estado => estado.SolicitudId == Id).
                                OrderByDescending(estado => estado.Fecha).
                                ToListAsync();
            var ultimo = entidadesEstado.OrderByDescending(x => x.Fecha).FirstOrDefault();


            //nuevoEstadoDto.Fecha = DateTime.Now;
            //nuevoEstadoDto.EstadoActual = "Pendiente";
            //var entidadEstado = mapper.Map<Estado>(nuevoEstadoDto);
            //context.Add(entidadEstado);
            //await context.SaveChangesAsync();
            //var estadoDto = mapper.Map<EstadoDTO>(entidadEstado);

            //return new CreatedAtRouteResult("ObtenerEstado", new { id = estadoDto.Id }, estadoDto);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {

            var existe = await context.Estados.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Estado() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }

    }
}
