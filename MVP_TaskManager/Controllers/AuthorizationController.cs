using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVP_TaskManager.Classes;
using MVP_TaskManager.Data;
using MVP_TaskManager.DTO;

namespace MVP_TaskManager.Controllers
{
    [ApiController]
    [Route("api/Authorization")]
    public class AuthorizationController : ControllerBase
    {
        private readonly Operations_authorization operations;

        public AuthorizationController (Operations_authorization _operations)
        {
            operations = _operations;    
        }

        [AllowAnonymous]
        [HttpPost] // Регистрация
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto) // Метод принимает объект от пользователя в формате json, где есть все входные данные
        {
            var result = await operations.Registration(dto);

            if (result == false) {
                return BadRequest("Пользователь с таким логином уже существует!");
            }
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login")] //Логирование
        public async Task<ActionResult<AuthResponseDTO>> Login(
        [FromBody] LoginDTO dto)
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
