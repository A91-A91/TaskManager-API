using System.ComponentModel.DataAnnotations;

namespace MVP_TaskManager.DTO
{
    public class Task_updateDTO
    {

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? IdStatus { get; set; }
    }
}
