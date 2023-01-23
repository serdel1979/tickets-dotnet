using System.ComponentModel.DataAnnotations;

namespace tickets.DTOs
{
    public class EditarAdminDTO
    {
        [Required]
        public string UserName { get; set; }
    }
}
