using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MVP_TaskManager.Classes;
using MVP_TaskManager.DTO;
using MVP_TaskManager.Models;
namespace MVP_TaskManager.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{

    private Operations_users operations;
    public UsersController(Operations_users _operations)
    {
        operations = _operations;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll()
    {
        var all = await operations.AllUser();
        return Ok(all);
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("{id_user}")]
    public async Task<ActionResult<User>> GetById(int id_user)
    {
        var user = await operations.UsersForID(id_user);
        return Ok(user);    
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser([FromQuery]
    User_CreateDTO user)
    {
        var newUser = await operations.CreateNewUser(user);

        return Ok(newUser);
    }

    [Authorize]
    [HttpDelete] // Удаление пользователей
    public async Task<ActionResult<User>> DeleteUser( 
    int id_user)
    {
        var isAdmin = User.IsInRole("Admin");
        var id_User_Deleting = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var delUser = await operations.DeleteUser(id_user, isAdmin,id_User_Deleting);
        return Ok(delUser);
    }

    [Authorize]
    [HttpPatch] //Обновление пользователей
    public async Task<ActionResult<User>> UpdateUser([FromQuery] int id_user,
    [FromQuery] User_UpdateDTO user)
    {
        var isAdmin = User.IsInRole("Admin");
        var id_User_id_User_Updating = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var updateUser = await operations.UpdateUser(id_user, user, id_User_id_User_Updating, isAdmin);
       
        return Ok(updateUser);
    }
}

