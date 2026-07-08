namespace MVP_TaskManager.Models
{
    public class Tasks
    {
        public int Id_task { get; set; }
        public int id_user { get; set; }
        public string Name_task { get; set; } = string.Empty;
        public string Description_task { get; set; } = string.Empty;
    }
}
