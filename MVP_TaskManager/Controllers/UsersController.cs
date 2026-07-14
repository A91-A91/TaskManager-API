using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MVP_TaskManager.Classes;
using MVP_TaskManager.Models;

namespace MVP_TaskManager.Controllers;


[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    
    private static List<User> users = new();
    private static int nextId = 1;
    private Operations operations;
    public UsersController(Operations _operations)
    {
        operations = _operations;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll()
    {
        var all = await operations.AllUser();
        return Ok(all);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Models.User>> GetById(int id_user)
    {
        var user = await operations.UsersForID(id_user);
        return Ok(user);
        
    }


    [HttpPost]
    public async Task<ActionResult<Models.User>> CreateUser(
    UserDTO user)
    {
        var newUser = await operations.CreateNewUser(user);

        return Ok(newUser);
    }

    [HttpDelete] //доделать
    public async Task<ActionResult<Models.User>> DeleteUser( 
    int id_user)
    {
        var delUser = await operations.DeleteUser(id_user);

        return Ok(delUser);
    }

    [HttpPut] //Обновление
    public async Task<ActionResult<Models.User>> UpdateUser(int id_user,
   UserDTO user)
    {
        var updateUser = await operations.UpdateUser(id_user, user);

        return Ok(updateUser);
    }
}

