namespace MVP_TaskManager.Models
{
    public class TaskDTO
    {
        public int IdTask { get; set; }

        public int? IdUser { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? IdStatus { get; set; }

        public DateOnly? DateCreate { get; set; }
    }
}
