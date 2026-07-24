using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVP_TaskManager.Data;
using MVP_TaskManager.Models;
using System.Runtime.CompilerServices;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using MVP_TaskManager.DTO;
namespace MVP_TaskManager.Classes
{
    public class Operations_tasks
    {
        private readonly TaskManagerContext context;

        public Operations_tasks(TaskManagerContext _context)
        {
            context = _context;
        }


        /// <summary>
        /// Возвращает все задачи пользователя
        /// </summary>
        /// <param name="id_user"></param>
        /// <returns></returns>
        public async Task<List<Models.Task>> ReturnAllTasks(string id_user) //!!!!
        {
            try
            {
                var task = await context.Tasks
                    .Where(user_task => user_task.IdUser == int.Parse(id_user))
                    .ToListAsync();
                return task;
            }
            catch { return null; }
        }

        public async Task <Models.Task> CreateNewTask(TaskDTO task, int id_user) //!!!!
        {

                var newTask = new Models.Task //создали новую задачу
                {
                    Name = task.Name,
                    Description = task.Description,
                    DateCreate = DateOnly.FromDateTime(DateTime.UtcNow),
                    IdUser = id_user, //разобраться, как убрать это при добавлении
                    IdStatus = task.IdStatus
                };
               
                context.Tasks.Add(newTask);
                await context.SaveChangesAsync();
                return newTask;
        }

        public async Task<bool> DeleteTask(int id_task, string idUser)
        {

            var taskForDel = await context.Tasks
                .FirstOrDefaultAsync(DelTask => DelTask.IdTask == id_task && DelTask.IdUser == int.Parse(idUser));
            
            if (taskForDel == null) return false;

            context.Tasks.Remove(taskForDel);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateTaks(int id_task, TaskDTO task)
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
    }
}
