using System.ComponentModel.DataAnnotations;
using MVP_TaskManager.Models;

namespace MVP_TaskManager.DTO
{
    public class User_CreateDTO
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Login { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public UserRole Role { get; set; }
    }
}
