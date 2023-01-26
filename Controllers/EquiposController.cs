using AutoMapper;
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
    [Route("api/equipos")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EquiposController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public EquiposController(ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }



        [HttpGet]
        [Authorize(Policy = "EsAdmin")]
        public async Task<ActionResult<List<EquipoDTO>>> GetEquipos([FromQuery] PaginacionDTO paginacion)
        {
            var queryable = context.Equipos.AsQueryable();
            await HttpContext.InsertaPaginacionEnCabecera(queryable);
            var entidadesEquipo = await queryable.OrderBy(eq => eq.Nombre).
                    Paginar(paginacion).ToListAsync();
            return mapper.Map<List<EquipoDTO>>(entidadesEquipo);
        }

        [HttpPost]
        [Authorize(Policy = "EsAdmin")]
        public async Task<ActionResult<RespuestaAutenticacion>> Post(EquipoDTO equipoDto)
        {
            var entidadEquipo = await context.Equipos.FirstOrDefaultAsync(eq => (eq.Inventario == equipoDto.Inventario));
            if (entidadEquipo != null)
            {
                return StatusCode(400,$"Ya existe el inventario {equipoDto.Inventario}");
            }
            var entidadEq = mapper.Map<Equipo>(equipoDto);
            context.Add(entidadEq);
            await context.SaveChangesAsync();
            var eqDto = mapper.Map<EquipoDTO>(entidadEq);
            return Ok(eqDto);
        }

        [HttpPut("{Id:int}")]
        [Authorize(Policy = "EsAdmin")]
        public async Task<ActionResult> Put(EquipoDTO equipoDto, int Id)
        {

            var existe = await context.Equipos.AnyAsync(x => x.Id == Id);
            if (!existe)
            {
                return NotFound();
            }

            var equipoBD = await context.Equipos.FirstOrDefaultAsync(eq => (eq.Inventario == equipoDto.Inventario));
            if (equipoBD != null)
            {
                return StatusCode(400, $"Ya existe el inventario {equipoDto.Inventario}");
            }

            equipoBD = mapper.Map<Equipo>(equipoDto);
            equipoBD.Id = Id;

            context.Update(equipoBD);
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        [Authorize(Policy = "EsAdmin")]
        public async Task<ActionResult> Delete(int id)
        {

            var existe = await context.Equipos.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Equipo() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }




    }
}
