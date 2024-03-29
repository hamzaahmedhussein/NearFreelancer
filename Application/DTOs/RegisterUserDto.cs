using Connect.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Connect.Application.DTOs
{
    public class RegisterUserDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public String PhoneNumber { get; set; }

        public string Email { get; set; }
        [Required]

        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Location { get; set; }
        public Gender Gender { get; set; }
        public DateTime DOB { get; set; }
    }
}
