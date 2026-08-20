namespace MVP_TaskManager.DTO
{
    public class TaskFilterDTO
    {
        public string? Name { get; set; }

        public DateOnly? StartDateFrom { get; set; }
        public DateOnly? StartDateTo { get; set; }

        public int? IdStatus { get; set; }
    }
}
