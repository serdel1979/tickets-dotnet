namespace tickets.DTOs
{
    public class RespuestaAutenticacion
    {
        public Boolean Ok { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }

    }
}
