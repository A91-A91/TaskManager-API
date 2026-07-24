using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVP_TaskManager.Classes;
using MVP_TaskManager.Data;
using MVP_TaskManager.DTO;

namespace MVP_TaskManager.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    public class AutController : ControllerBase
    {
        private readonly Operations_authorization operations;

        public AutController (Operations_authorization _operations)
        {
            operations = _operations;    
        }

        [AllowAnonymous]
        [HttpPost] // Регистрация
        public async Task<IActionResult> Register(RegisterDTO dto) // Метод принимает объект от пользователя в формате json, где есть все входные данные
        {
            var newUser = await operations.Registration(dto);

            if (newUser == null) {
                return BadRequest("Пользователь с таким логином уже существует!");
            }
            return Ok(newUser);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(
        LoginDTO dto)
        {
            var result = await operations.Login(dto);

            if (result == null)
            {
                return Unauthorized("Неверный логин или пароль");
            }

            return Ok(result);
        }

    }
}
