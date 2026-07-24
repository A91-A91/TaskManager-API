namespace MVP_TaskManager.DTO
{
    public class RegisterDTO
    {
        public string Username { get; set; } = null!; //сообщаем компилятору,
                                                      //что не выражение не будет пустым
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;

    }
}
