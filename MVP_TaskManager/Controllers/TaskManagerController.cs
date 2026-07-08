using Microsoft.AspNetCore.Mvc;
using MVP_TaskManager.Models;

namespace MVP_TaskManager.Controllers;

[ApiController]
[Route("api/users")]
public class TaskManagerController : ControllerBase
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
    public ActionResult<User> Create(User user)
    {
        user.Id = nextId++;
        users.Add(user);
        return user;
    }
}