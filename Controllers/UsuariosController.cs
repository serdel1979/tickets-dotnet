using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using tickets.DTOs;

namespace tickets.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signin;
        private readonly IDataProtector dataProtector;

        public UsuariosController(UserManager<IdentityUser> userManager, 
            IConfiguration configuration,
            SignInManager<IdentityUser> signin,
            IDataProtectionProvider dataProtectionProvider
            )
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signin = signin;
            dataProtector = dataProtectionProvider.CreateProtector("valor_re_secreto");
        }



        [HttpPost("registrar")]
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


        private async Task<RespuestaAutenticacion> construirToken(CredencialesUsuario credencialUsuario)
        {
            var claims = new List<Claim>(){
                new Claim("usuario", credencialUsuario.Usuario)
            };

            var usuario = await userManager.FindByNameAsync(credencialUsuario.Usuario);

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


    }
}
