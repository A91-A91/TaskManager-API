namespace MVP_TaskManager.Models
{
    public class UserDTO
    {
        //public int Id { get; set; }
        public string Username { get; set; } = null!;

        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;
        public DateOnly? RegDate { get; set; }

    }
}
