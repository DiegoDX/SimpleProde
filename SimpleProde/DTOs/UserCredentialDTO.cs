using System.ComponentModel.DataAnnotations;

namespace SimpleProde.DTOs
{
    public class UserCredentialDTO
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
