using System.ComponentModel.DataAnnotations;

namespace ZIMS.Models.Users
{
    public class AuthenticateRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}