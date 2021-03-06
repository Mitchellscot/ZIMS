using System.ComponentModel.DataAnnotations;

namespace ZIMS.Models.Users
{
    //not used yet
    public class AddUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public string Role { get; set; }
    }
}