using System.ComponentModel.DataAnnotations;

namespace StudentApi.Models
{
    public class RoleDTO
    {
        public int Id { get; set; }
        [Required]
        public string RoleName { get; set; }
        [Required]
        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
