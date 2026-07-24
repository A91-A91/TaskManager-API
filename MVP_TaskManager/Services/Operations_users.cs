using Microsoft.EntityFrameworkCore;
using MVP_TaskManager.Data;
using MVP_TaskManager.DTO;
using MVP_TaskManager.Models;
using AsyncTask = System.Threading.Tasks;

namespace MVP_TaskManager.Classes
{
    public class Operations_users
    {
        private readonly TaskManagerContext context;


        public Operations_users(TaskManagerContext _context)
        {
            context = _context;
        }

        public async Task<User> CreateNewUser(UserDTO user)
        {

            var newUser = new User
            {
                Username = user.Username,
                Login = user.Login,
                Password = user.Password,
                RegDate = DateOnly.FromDateTime(DateTime.UtcNow),
            };

            context.Users.Add(newUser);
            await context.SaveChangesAsync();
            return newUser;
        }

        public async Task<bool> UpdateUser(int id_user, UserDTO user)
        {
            var userForUpdate = await context.Users
                .FindAsync(id_user);

            if (userForUpdate == null) return false;

            userForUpdate.Username = user.Username;
            userForUpdate.Password = user.Password;
            userForUpdate.RegDate = user.RegDate;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> AllUser()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<List<User>> UsersForID(int id_user)
        {
            var userByID = await context.Users.Where(userID => userID.Id == id_user).ToListAsync();
            return userByID;
        }

        public async Task<bool> DeleteUser(int id_user)
        {
            var userForDel = await context.Users
                .FindAsync(id_user);

            if (userForDel == null) return false;

            context.Users.Remove(userForDel);
            await context.SaveChangesAsync();
            return true;
        }

    }
}
