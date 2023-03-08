using System.ComponentModel.DataAnnotations;

namespace TodoAppWeb.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public string? PasswordConfirmation { get; set; }

        [Required]
        public string? Email { get; set; }
    }
}
