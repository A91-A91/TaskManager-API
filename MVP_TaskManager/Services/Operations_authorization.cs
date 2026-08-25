using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MVP_TaskManager.Data;
using MVP_TaskManager.DTO;
using MVP_TaskManager.Models;
using Npgsql.TypeMapping;

namespace MVP_TaskManager.Classes
{
    public class Operations_authorization
    {
        private readonly TaskManagerContext context;
        private readonly IConfiguration configuration;

        public Operations_authorization(
            TaskManagerContext context,
            IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        /// <summary>
        /// Метод регистрации нового пользователя
        /// </summary>
        /// <param name="user">Объект типа USER</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// 
        public async Task<bool> Registration( RegisterDTO user) //создание нового пользователя (регистрация)
        {
            var exists = await context.Users
            .AnyAsync(x => x.Login == user.Login);

            if (exists)
                throw new InvalidOperationException
                    ("Такой логин уже существует!");

            var newUser = new User
            {
                Username = user.Username,
                Login = user.Login,
                Password = user.Password,
            };

            newUser.RegDate = DateOnly.FromDateTime(DateTime.UtcNow);
            newUser.Role = UserRole.User; 

            context.Users.Add(newUser);
            await context.SaveChangesAsync();
            return true;
        }


        /// <summary>
        /// Логирование пользователя
        /// </summary>
        /// <param name="logDTO"></param>
        /// <returns></returns>
        public async Task<AuthResponseDTO?> Login(LoginDTO logDTO)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(x => x.Login == logDTO.Login);

            if (user == null || user.Password != logDTO.Password)
            {
                return null;
            }

            var token = CreateToken(user);

            return new AuthResponseDTO
            {
                Token = token
            };
        }

        /// <summary>
        /// Создание токена
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns></returns>
        private string CreateToken(User user)
        {
            var claims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
               new Claim(ClaimTypes.Name, user.Username!),
               new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
       
    }
}
