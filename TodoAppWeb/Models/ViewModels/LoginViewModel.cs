using System.ComponentModel.DataAnnotations;

namespace TodoAppWeb.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
