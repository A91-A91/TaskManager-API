using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVP_TaskManager.Classes;
using MVP_TaskManager.Models;
//using MVP_TaskManager.Models ;

namespace MVP_TaskManager.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        
        private readonly Operations operations;

        public TasksController(Operations _operations) 
        {
            operations = _operations;
        }

        [HttpGet("{id_user}")] //поиск и вывод задач у определенного пользователя
        public async Task<ActionResult <List<Models.Task>>> GetAllTaskByUser(int id_user)
        {
           var tasks = await operations.ReturnAllTasks(id_user);
           return Ok(tasks);
        }


        [HttpPost] // Создание задачи
        public async Task<IActionResult> CreateTask(Models.TaskDTO task) // Метод принимает объект от пользователя в формате json, где есть все входные данные
        {
 
                var newTask = await operations.CreateNewTask(task);
                return Ok(newTask);
        }

        [HttpDelete("{id_task}")] // Удаление задачи
        public async Task<IActionResult> CreateTask(int id_task) // Метод принимает объект от пользователя в формате json, где есть все входные данные
        {

            var delTask = await operations.DeleteTask(id_task);
            return Ok(delTask);
        }

        [HttpPut("{id_task}")] // Изменение задачи
        public async Task<IActionResult> UpdateTask(int id_task, TaskDTO task) // Метод принимает объект от пользователя в формате json, где есть все входные данные
        {

            var updateTask = await operations.UpdateTaks(id_task, task);
            return Ok(updateTask);
        }
    }
}
