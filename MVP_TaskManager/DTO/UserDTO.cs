using MVP_TaskManager.Models;

namespace MVP_TaskManager.DTO
{
    public class UserDTO
    {
       
        public string Username { get; set; } = null!;

        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;
        public UserRole Role { get; set; } 
        
        public DateOnly? RegDate { get; set; }

    }
}
