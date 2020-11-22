using System.ComponentModel.DataAnnotations;

namespace MinimalBlazorAdmin.Shared.Models
{
    public class LoginDto
    {
        [Required]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "user name length should in 4-20")]
        public string Name { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "password length should more than 6")]
        public string Password { get; set; }
    }
}