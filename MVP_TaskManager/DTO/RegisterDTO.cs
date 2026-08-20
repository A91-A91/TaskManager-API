using System.ComponentModel.DataAnnotations;

namespace MVP_TaskManager.DTO
{
    public class RegisterDTO
    {

        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        public string Username { get; set; } = null!; //сообщаем компилятору,
                                                      //что не выражение не будет пустым

        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        public string Login { get; set; } = null!;

        [Required]
        [MinLength(6)]
        [MaxLength(15)]
        public string Password { get; set; } = null!;

    }
}
