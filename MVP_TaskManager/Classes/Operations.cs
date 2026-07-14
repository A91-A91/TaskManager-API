using System;
using AsyncTask = System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVP_TaskManager.Data;
using MVP_TaskManager.Models;
using System.Runtime.CompilerServices;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
namespace MVP_TaskManager.Classes
{
    public class Operations
    {
        //private static List<Models.Task> tasks = new();
        private static int nextId = 1;
        private static List<User> users = new();
        private readonly TaskManagerContext context;

        public Operations(TaskManagerContext _context)
        {
            context = _context;
        }


        /// <summary>
        /// Возвращает все задачи пользователя
        /// </summary>
        /// <param name="id_user"></param>
        /// <returns></returns>
        public async AsyncTask.Task<List<Models.Task>> ReturnAllTasks(int id_user)
        {
            try
            {
                var task = await context.Tasks
                    .Where(user_task => user_task.IdUser == id_user)
                    .ToListAsync(); //это список
 

                return task;
            }
            catch { return null; }
        }

        public async AsyncTask.Task <Models.Task> CreateNewTask(Models.TaskDTO task)
        {
                var newTask = new Models.Task //создали новую задачу
                {
                    Name = task.Name,
                    Description = task.Description,
                    DateCreate = DateOnly.FromDateTime(DateTime.UtcNow),
                    IdUser = task.IdUser,
                    IdStatus = task.IdStatus
                };
               
                context.Tasks.Add(newTask);
                await context.SaveChangesAsync();
                return newTask;
        }

        public async AsyncTask.Task<bool> DeleteTask(int id_task)
        {
            var taskForDel = await context.Tasks
                .FirstOrDefaultAsync(DelTask => DelTask.IdTask == id_task);

            if (taskForDel == null) return false;

            context.Tasks.Remove(taskForDel);
            await context.SaveChangesAsync();
            return true;
        }

        public async AsyncTask.Task<bool> UpdateTaks(int id_task, TaskDTO task)
        {
            var taskForUpdate = await context.Tasks
                .FindAsync(id_task);

            if (taskForUpdate == null) return false;

            taskForUpdate.Name = task.Name;
            taskForUpdate.Description = task.Description;
            taskForUpdate.IdStatus = task.IdStatus;
            taskForUpdate.DateCreate = task.DateCreate;

            await context.SaveChangesAsync();
            return true;
        }

        public async AsyncTask.Task <Models.User> CreateNewUser(UserDTO user)
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

        public async AsyncTask.Task<bool> UpdateUser(int id_user, UserDTO user)
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

        public async AsyncTask.Task<List<Models.User>> AllUser()
        {
            return await context.Users.ToListAsync();
        }

        public async AsyncTask.Task<List<Models.User>> UsersForID(int id_user)
        {
            var userByID = await context.Users.Where(userID => userID.Id == id_user).ToListAsync();
            return userByID;
        }

        public async AsyncTask.Task<bool> DeleteUser(int id_user)
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
