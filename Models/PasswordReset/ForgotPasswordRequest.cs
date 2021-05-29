using System.ComponentModel.DataAnnotations;

namespace ZIMS.Models.PasswordReset
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}