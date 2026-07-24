namespace MVP_TaskManager.DTO
{
    public class AuthResponseDTO //для отправки пользователю инфы при успешной авторизации
    {
        public string Token { get; set; } = null!;
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Login { get; set; } = null!;
    }
}
