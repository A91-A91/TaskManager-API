using Microsoft.EntityFrameworkCore;
using MVP_TaskManager.Data;
using MVP_TaskManager.DTO;
using MVP_TaskManager.Models;
using AsyncTask = System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Diagnostics.Contracts;

namespace MVP_TaskManager.Classes
{
    public class Operations_users
    {
        private readonly TaskManagerContext context;
        public Operations_users(TaskManagerContext _context)
        {
            context = _context;
        }

        public async Task<User> CreateNewUser(User_CreateDTO user)
        {
            
            if (Enum.IsDefined(typeof(UserRole), "Admin") == false)
            {
                throw new InvalidOperationException("Такой роли нет!");
            }

            var newUser = new User
            {
                Username = user.Username,
                Login = user.Login,
                Password = user.Password,
                RegDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Role = UserRole.User,
            };

            if (await CheckLogin(newUser.Login))
            {
                throw new InvalidOperationException("Такой логин уже существует!");
            }

            context.Users.Add(newUser);
            await context.SaveChangesAsync();
            return newUser;
        }



        public async Task<bool> UpdateUser(int id_user, 
            [FromQuery] User_UpdateDTO user, 
            int id_User_Updating, 
            bool is_Admin)
        {
            var userForUpdate = await context.Users
                .FindAsync(id_user);

            if (userForUpdate == null) return false;

            if (user.Username != null)
                userForUpdate.Username = user.Username;

            if (user.Password != null)
                userForUpdate.Password = user.Password;

            if (CheckRoots(id_user, is_Admin, id_User_Updating) == false)
            {
                throw new InvalidOperationException
                    ("Недостаточно прав на эту операцию!");
            }

            await context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> CheckLogin(string login)
        {
            return await context.Users.AnyAsync(u => u.Login == login);
        }

        public async Task<List<User>> AllUser(int page, int pageSize)
        {   
            var query = context.Users.AsQueryable();

            if (!CheckValue(page,pageSize))
            {
                throw new InvalidOperationException
                    ("Невозможные знания для page или pageSize");
            }

            query = query
               .Skip((page - 1) * pageSize)
               .Take(pageSize);
            return await query.ToListAsync();
        }

        private bool CheckValue(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0) { return false; }
            else
                return true; 
        }

        public async Task<List<User>> UsersForID(int id_user)
        {
            var userByID = await context.Users.Where(userID => userID.Id == id_user).ToListAsync();
            return userByID;
        }

        public async Task<bool> DeleteUser(int id_user, bool is_Admin, int id_User_Deleting )
        {
            
            var userForDel = await context.Users
                .FirstOrDefaultAsync(u => u.Id == id_user);

            if (userForDel == null) return false;

            if (CheckRoots(id_user, is_Admin, id_User_Deleting) == false)
            {
                throw new UnauthorizedAccessException();
            }

            await DeleteAllTasksByUser(id_user);
            context.Users.Remove(userForDel);
            
            await context.SaveChangesAsync();
            return true;
        }

        public bool CheckRoots(int? id_user, bool is_Admin, int id_User_Required)
        {
            if (id_user == id_User_Required || is_Admin)
            {
                return true;
            }
            else return false;
        }

        private async Task<bool> DeleteAllTasksByUser (int id_user)
        {
            var tasks = await context.Tasks
                .Where(t => t.IdUser == id_user)
                .ToListAsync();

            if(tasks.Count == 0)
                return false;

            context.Tasks.RemoveRange(tasks);
            await context.SaveChangesAsync();
            return true;
        }

    }
}
