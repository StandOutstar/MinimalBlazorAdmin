using System.ComponentModel.DataAnnotations;

namespace MinimalBlazorAdmin.Shared.Models
{
    public class LoginDto
    {
        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "user name length should in 6-20")]
        public string Name { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "password length should more than 8")]
        public string Password { get; set; }
    }
}