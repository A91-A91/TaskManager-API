using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVP_TaskManager.Classes;
using MVP_TaskManager.DTO;

//using MVP_TaskManager.Models ;

namespace MVP_TaskManager.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        
        private readonly Operations_tasks operations;

        public TasksController(Operations_tasks _operations) 
        {
            operations = _operations;
        }

        [Authorize]
        [HttpGet] //поиск и вывод задач у определенного пользователя
        public async Task<ActionResult <List<Models.Task>>> GetAllTaskByUser()
        {
            try
            {
                var id_user = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var tasks = await operations.ReturnAllTasks(id_user!);
                return Ok(tasks);
            }
            catch { return Conflict(); }
        }

       
        [Authorize]
        [HttpPost] // Создание задачи у определённого юзера
        public async Task<IActionResult> CreateTask(TaskDTO task) // Метод принимает объект от пользователя в формате json, где есть все входные данные
        {
            try
            {
                var id_user = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var newTask = await operations.CreateNewTask(task, int.Parse(id_user!));
                return Ok(newTask);
            }
            catch { return Conflict(); }
        }


        
       [Authorize]
       [HttpDelete("{id_task}")] // Удаление задачи
       public async Task<IActionResult> CreateTask(int id_task) // Метод принимает объект от пользователя в формате json, где есть все входные данные
       {
            var id_user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var delTask = await operations.DeleteTask(id_task, id_user!);
           return Ok(delTask);
       }

        /*
       [Authorize]
       [HttpPut("{id_task}")] // Изменение задачи
       public async Task<IActionResult> UpdateTask(int id_task, TaskDTO task) // Метод принимает объект от пользователя в формате json, где есть все входные данные
       {

           var updateTask = await operations.UpdateTaks(id_task, task);
           return Ok(updateTask);
       }

       */
    }
}
