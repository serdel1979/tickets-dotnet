using Microsoft.AspNetCore.Identity;

namespace tickets.Entidades
{
    public class Usuario : IdentityUser
    {
        public Boolean habilitado { get; set; } = false;
    }
}
