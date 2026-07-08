using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MVP_TaskManager.Models;

namespace MVP_TaskManager.Controllers;


[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private static List<User> users = new();
    private static int nextId = 1;


    [HttpGet]
    public ActionResult<List<User>> GetAll()
    {
        return users;
    }


    [HttpGet("{id}")]
    public ActionResult<User> GetById(int id)
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user == null)
            return NotFound();

        return user;
    }


    [HttpPost]
    public ActionResult<User> CreateUser(User user)
    {
        user.Id = nextId++;
        users.Add(user);
        return user;
    }
}

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private static List<User> users = new();
    private static List<Tasks> tasks = new();
    private static int nextId = 1;

    [HttpGet("{id_user}")] //поиск задач у определенного пользователя
    public ActionResult<List<Tasks>> GetAllTaskByUser(int id_user)
    {
        //var task = tasks.FirstOrDefault(user_task => user_task.id_user == id_user);
        var task = tasks.Where(user_task => user_task.id_user == id_user).ToList(); //это список
        if (task == null)
            return NotFound();

        return task;
    }

    [HttpPost] // Создание задачи
    public IActionResult CreateTask(Tasks task) // Метод принимает объект от пользователя в формате json, где есть все входные данные
    {
        try
        {
            tasks.Add(new Tasks
            {
                Id_task = nextId++,
                Name_task = task.Name_task,
                Description_task = task.Description_task,
                id_user = task.id_user,
            });
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound();
        }
    }
}