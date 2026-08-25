using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVP_TaskManager.Classes;
using MVP_TaskManager.Data;
using MVP_TaskManager.DTO;
using MVP_TaskManager.Models;



namespace MVP_TaskManager.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly TaskManagerContext context;
        private readonly Operations_tasks operations;

        public TasksController(TaskManagerContext _context,
            Operations_tasks _operations)
        {
            context = _context;
            operations = _operations;
        }

        [Authorize]
        [HttpGet] //поиск и вывод задач у определенного пользователя
        public async Task<ActionResult<List<Models.Task>>> GetAllTaskByUser(
          [FromQuery] TaskFilterDTO filter,
          [FromQuery] TaskSortDTO sort, int page = 1)
        {
            var id_user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var tasks = await operations.ReturnAllTasks(int.Parse(id_user!), filter, sort, page);
            return Ok(tasks);
        }


        [Authorize]
        [HttpPost] // Создание задачи у определённого юзера
        public async Task<IActionResult> CreateTask([FromBody] TaskDTO task) // Метод принимает объект от пользователя в формате json, где есть все входные данные
        {
            var id_user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newTask = await operations.CreateNewTask(task, int.Parse(id_user!));
            return Ok(newTask);
        }

        [Authorize]
        [HttpDelete] // Удаление задачи
        public async Task<IActionResult> DeleteTask([FromQuery] int id_task) // Метод принимает объект от пользователя в формате json, где есть все входные данные
        {
            var delTask = await operations.DeleteTask(id_task);
            return Ok(delTask);
        }

        [Authorize]
        [HttpPatch]
        public async Task<bool> UpdateTask([FromQuery] int id_task, [FromQuery] Task_updateDTO task)
        {
            var isAdmin = User.IsInRole(UserRole.Admin.ToString());
            var id_user = int.Parse(User.FindFirstValue(
                ClaimTypes.NameIdentifier)!);
            var updateTask = await operations.UpdateTask(id_task, task,id_user,isAdmin);
            await context.SaveChangesAsync();
            return true;
        }

        [Authorize]
        [HttpGet("Status")]
        public async Task<IActionResult> GetStatus()
        {
            var statuses = await operations.GetAllStatus();
            return Ok(statuses);
        }
    }
}
