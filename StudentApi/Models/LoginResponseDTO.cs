using System.ComponentModel.DataAnnotations;

namespace StudentApi.Models
{
    public class LoginResponseDTO
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string token { get; set; }

        [Required]
        public string Username { get;  set; }

     
    }
}
