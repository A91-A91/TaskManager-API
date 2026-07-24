using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MVP_TaskManager.Classes;
using MVP_TaskManager.DTO;
using MVP_TaskManager.Models;
namespace MVP_TaskManager.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{

    private Operations_users operations;
    public UsersController(Operations_users _operations)
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
    public async Task<ActionResult<User>> GetById(int id_user)
    {
        var user = await operations.UsersForID(id_user);
        return Ok(user);
        
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(
    UserDTO user)
    {
        var newUser = await operations.CreateNewUser(user);

        return Ok(newUser);
    }

 
    [HttpDelete] // Удаление пользователей
    public async Task<ActionResult<User>> DeleteUser( 
    int id_user)
    {
        var delUser = await operations.DeleteUser(id_user);

        return Ok(delUser);
    }

    [HttpPut] //Обновление
    public async Task<ActionResult<User>> UpdateUser(int id_user,
   UserDTO user)
    {
        var updateUser = await operations.UpdateUser(id_user, user);

        return Ok(updateUser);
    }
}

