using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using tickets.DTOs;
using tickets.Utilidades;

namespace tickets.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signin;
        private readonly IMapper mapper;
        private readonly IDataProtector dataProtector;

        public UsuariosController(UserManager<IdentityUser> userManager,
            ApplicationDbContext context,
            IConfiguration configuration,
            SignInManager<IdentityUser> signin,
            IMapper mapper,
            IDataProtectionProvider dataProtectionProvider
            )
        {
            this.userManager = userManager;
            this.context = context;
            this.configuration = configuration;
            this.signin = signin;
            this.mapper = mapper;
            dataProtector = dataProtectionProvider.CreateProtector("valor_re_secreto");
        }



        [HttpPost("registrar")]
        [AllowAnonymous]
        public async Task<ActionResult<RespuestaAutenticacion>> Login(RegistroUsuario credenciales)
        {
            var usuario = new IdentityUser
            {
                UserName = credenciales.Usuario,
                Email = credenciales.Email
            };

            var resultado = await userManager.CreateAsync(usuario, credenciales.Password);
            if (resultado.Succeeded)
            {
                return Ok();
            }
            return BadRequest(resultado.Errors);
        }




        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<RespuestaAutenticacion>>Registrar(CredencialesUsuario credenciales)
        {
            var resultado = await signin.PasswordSignInAsync(credenciales.Usuario, credenciales.Password,
                isPersistent: false, lockoutOnFailure: false);
            if (resultado.Succeeded)
            {
                return await construirToken(credenciales);
            }
            return BadRequest("Login incorrecto");
        }

        [HttpGet]
        [Authorize(Policy = "EsAdmin")]
        public async Task<ActionResult<List<UsuarioDTO>>> GetUsuarios([FromQuery] PaginacionDTO paginacion)
        {
            var queryable = context.Users.AsQueryable();
            await HttpContext.InsertaPaginacionEnCabecera(queryable);
            var entidadesUsuarios = await queryable.OrderBy(x => x.UserName).Paginar(paginacion).ToListAsync();
            var usuariosDTO = mapper.Map<List<UsuarioDTO>>(entidadesUsuarios);

            return Ok(usuariosDTO);
        }

        private async Task<RespuestaAutenticacion> construirToken(CredencialesUsuario credencialUsuario)
        {


            var usuario = await userManager.FindByNameAsync(credencialUsuario.Usuario);

            var claims = new List<Claim>(){
                new Claim("usuario", usuario.Id)
            };

            var claimsDB = await userManager.GetClaimsAsync(usuario);

            claims.AddRange(claimsDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Secret"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddDays(1);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: creds);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion
            };

        }

        [HttpPost("hacerAdmin")]
        public async Task<ActionResult<RespuestaAutenticacion>> hacerAdmin(EditarAdminDTO editarAdminDTO)
        {
            var usuario = await userManager.FindByNameAsync(editarAdminDTO.UserName);

            await userManager.AddClaimAsync(usuario, new Claim("esAdmin", "1"));
            return NoContent();
        }

        [HttpPost("borrarAdmin")]
        public async Task<ActionResult<RespuestaAutenticacion>> borrarAdmin(EditarAdminDTO editarAdminDTO)
        {
            var usuario = await userManager.FindByNameAsync(editarAdminDTO.UserName);

            await userManager.RemoveClaimAsync(usuario, new Claim("esAdmin", "1"));
            return NoContent();
        }
    }
}
