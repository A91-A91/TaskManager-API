using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVP_TaskManager.Data;
using MVP_TaskManager.Models;
using System.Runtime.CompilerServices;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using MVP_TaskManager.DTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;


namespace MVP_TaskManager.Classes
{
    public class Operations_tasks
    {
        private readonly TaskManagerContext context;
        private readonly Operations_users operations;

        public Operations_tasks(TaskManagerContext _context, Operations_users operations_)
        {
            context = _context;
            operations = operations_;
        }


        /// <summary>
        /// Возвращает все задачи пользователя
        /// </summary>
        /// <param name="id_user"></param>
        /// <returns></returns>
        public async Task<List<Models.Task>> ReturnAllTasks(int? id_user, 
        TaskFilterDTO filterTask, TaskSortDTO sort,
        int page, int pageSize = 2)
            {
            try
            {
              
                var query = context.Tasks.AsQueryable(); //вывод записей из Tasks

                query = GetTaskOfUser(query, id_user);
                query = FilterByNameTask(query, filterTask); //добавляем сам фильтр
                query = FilterByDate(query, filterTask);
                query = FilterByStatus(query, filterTask);
                query = SortByDate(sort, query);
                query = SortByName(sort, query);
                
                //Пагинация ?
                query = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

                return await query.ToListAsync();

            }
            catch { return null; }
        }

        /// <summary>
        /// Создание новых задач
        /// </summary>
        /// <param name="task">Объект задачи</param>
        /// <param name="id_user">ID пользователя</param>
        /// <returns></returns>
        public async Task <Models.Task> CreateNewTask(TaskDTO task, int id_user) //!!!!
        {
            var newTask = new Models.Task //создали новую задачу
            {
                Name = task.Name,
                Description = task.Description,
                IdStatus = task.IdStatus
            };

            newTask = await CheckExistInfo(task, newTask);
            
            newTask.DateCreate = DateOnly.FromDateTime(DateTime.UtcNow); 
            newTask.IdUser = id_user; 

            context.Tasks.Add(newTask);
            await context.SaveChangesAsync();
            return newTask;
        }


        /// <summary>
        /// Проверка на существование статуса
        /// </summary>
        /// <param name="idStatus">Номер статуса</param>
        /// <returns></returns>
        private async Task<bool> CheckExistStatus(int? idStatus)
        {
            return await context.StatusRefs
                .AnyAsync(s => s.IdStatus == idStatus);
        }
        /// <summary>
        /// Проверка на пустоту введеных данных
        /// </summary>
        /// <param name="task"></param>
        /// <param name="newTask"></param>
        /// <returns></returns>
        private async Task<Models.Task> CheckExistInfo(TaskDTO task, Models.Task newTask)
        {
            if (!await CheckExistStatus(task.IdStatus))
            { newTask.IdStatus = (int)Task_Status.New; }

            if (string.IsNullOrWhiteSpace(task.Description))
            { newTask.Description = "Нет описания"; }

            if (string.IsNullOrWhiteSpace(task.Name))
            { newTask.Description = "Нет названия!"; }

            return newTask;
        }

        /// <summary>
        /// Метод вывода всех статусов из БД
        /// </summary>
        /// <returns></returns>
        public async Task<List<StatusRef>> GetAllStatus()
        {
            var statuses = await context.StatusRefs.ToListAsync();
            return statuses;
        }

        /// <summary>
        /// Удаление задачи 
        /// </summary>
        /// <param name="id_task"></param>
        /// <param name="id_user"></param>
        /// <param name="is_Admin"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public async Task<bool> DeleteTask(int id_task, int id_user, bool is_Admin)
        {

            var taskForDel = await context.Tasks
                .FirstOrDefaultAsync(DelTask => DelTask.IdTask == id_task);
            
            if (taskForDel == null) return false;

            if (!operations.CheckRoots(taskForDel.IdUser,
                is_Admin, id_user))
            {
                throw new UnauthorizedAccessException();
            }

            context.Tasks.Remove(taskForDel);
            await context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Обновление задачи
        /// </summary>
        /// <param name="id_task"></param>
        /// <param name="task"></param>
        /// <param name="id_User_Required"></param>
        /// <param name="is_Admin"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public async Task<bool> UpdateTask(int id_task,
            Task_updateDTO task,
            int id_User_Required,
            bool is_Admin) 
        {
            var taskForUpdate = await context.Tasks
                .FindAsync(id_task);

            if (taskForUpdate == null) return false;
            
            if (!operations.CheckRoots(taskForUpdate.IdUser, 
                is_Admin, id_User_Required))
            {
                throw new UnauthorizedAccessException();
            }

            if (task.Name != null)
            {
                taskForUpdate.Name = task.Name;
            }

            if (task.Description != null)
            {
                taskForUpdate.Description = task.Description;
            }
            ///можно в отдельный метод кинуть
            if (task.IdStatus != null)
            {
                taskForUpdate.IdStatus = task.IdStatus.Value;
            }

            await context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Фильтрация вывода записей
        /// </summary>
        /// <param name="filterTask"></param>
        /// <returns></returns>
        public async Task<List<Models.Task>> FiltersTask(TaskFilterDTO filterTask)
        {
            var query = context.Tasks.AsQueryable(); 

            query = ResearchByNameTask(query,filterTask); 
            query = FilterByDate(query,filterTask);
            query = FilterByStatus(query, filterTask);

            return await query.ToListAsync();
        }

        /// <summary>
        /// Поиск по названию задачи
        /// </summary>
        /// <param name="query"></param>
        /// <param name="filterTask"></param>
        /// <returns></returns>
        private IQueryable<Models.Task> ResearchByNameTask(IQueryable<Models.Task> query, TaskFilterDTO filterTask)
        {
            if (!string.IsNullOrWhiteSpace(filterTask.Name))
            {
                query = query.Where(r =>
                    r.Name!.Contains(filterTask.Name)); 
            }
            else { Console.WriteLine("Пустая строка для имени!"); }
            return query;
        }

        /// <summary>
        /// Фильтрация по дате создания
        /// </summary>
        /// <param name="query"></param>
        /// <param name="filterTask"></param>
        /// <returns></returns>

        private IQueryable<Models.Task> FilterByDate(IQueryable<Models.Task> query, TaskFilterDTO filterTask)
        {
            if (filterTask.StartDateFrom.HasValue)
            {
                query = query.Where(r =>
                    r.DateCreate >= filterTask.StartDateFrom.Value);
            }
            if (filterTask.StartDateTo.HasValue)
            {
                query = query.Where(r =>
                    r.DateCreate <= filterTask.StartDateTo.Value);
            }

            return query;
        }

        /// <summary>
        /// Фильтрация по статусу задачи
        /// </summary>
        /// <param name="query"></param>
        /// <param name="filterTask"></param>
        /// <returns></returns>
        private IQueryable<Models.Task> FilterByStatus(IQueryable<Models.Task> query, TaskFilterDTO filterTask)
        {
            if (filterTask.IdStatus.HasValue)
            {
                query = query.Where(r =>
                    r.IdStatus == filterTask.IdStatus);
            }
            return query;
        }

        /// <summary>
        /// Общий метод для вызова методов сортировок
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<Models.Task>> SortTask(TaskSortDTO filter)
        {
            var query = context.Tasks.AsQueryable();
            query = SortByDate(filter, query);
            query = SortByName(filter, query);
            return await query.ToListAsync();
        }

        /// <summary>
        /// Сортировка по дате создания задачи
        /// </summary>
        /// <param name="task"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private IQueryable<Models.Task> SortByDate(TaskSortDTO task, IQueryable<Models.Task> query)
        {
            if (task.SortBy == "date")
            {
                if (task.Desk == true)
                {
                    query = query.OrderByDescending
                        (task => task.DateCreate);
                }
                else {
                    query = query.OrderBy
                        (task => task.DateCreate);
                }
            }
            return query;
        }
        /// <summary>
        /// Сортировка по имена задачи
        /// </summary>
        /// <param name="task"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private IQueryable<Models.Task> SortByName(TaskSortDTO task, IQueryable<Models.Task> query)
        {
            if (task.SortBy == "name")
            {
                if (task.Desk == true)
                {
                    query = query.OrderByDescending
                        (task => task.Name);
                }
                else
                {
                    query = query.OrderBy
                        (task => task.Name);    
                }
            }
            return query;
        }

        /// <summary>
        /// Получение задачи определенного юзера (для запроса)
        /// </summary>
        /// <param name="query"></param>
        /// <param name="id_user"></param>
        /// <returns></returns>
        private IQueryable<Models.Task> GetTaskOfUser(
        IQueryable<Models.Task> query,
        int? id_user)
        {
            return query.Where(task => task.IdUser == id_user);
        }
    }
}

// Не знаю зачем я добавил комменты к методам, но пусть будут