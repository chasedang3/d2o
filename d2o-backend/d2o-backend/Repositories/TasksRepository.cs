using System.Data;
using Dapper;
using d2o_backend.Data;
using d2o_backend.Models;
using d2o_backend.Models.Entities;

namespace d2o_backend.Repositories
{
    public interface ITasksRepository 
    {
        public Task<IEnumerable<TodoTask>> GetAllTasks();
        public Task<TodoTask> GetTaskById(Guid id);
        public Task<TodoTask> AddTask(AddTaskDto task);
        public Task UpdateTask(Guid id, UpdateTaskDto task);
        public Task DeleteTask(Guid id);
    }

    public class TasksRepository : ITasksRepository
    {
        private readonly AppDbContext _context;

        public TasksRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TodoTask>> GetAllTasks()
        {
            var query = "SELECT * FROM [dbo].[TodoTasks] ORDER BY CreatedAt DESC";

            using (var connection = _context.CreateConnection())
            {
                var tasks = await connection.QueryAsync<TodoTask>(query);

                return tasks.ToList();
            }
        }

        public async Task<TodoTask> GetTaskById(Guid id)
        {
            var query = "SELECT * FROM [dbo].[TodoTasks] WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Guid);

            using (var connection = _context.CreateConnection())
            {
                var task = await connection.QuerySingleOrDefaultAsync<TodoTask>(query, parameters);

                return task;
            }
        }

        public async Task<TodoTask> AddTask(AddTaskDto task)
        {
            var query = "INSERT INTO [dbo].[TodoTasks] (Id, Name, IsCompleted, CreatedAt, UpdatedAt) " +
                        "VALUES (@Id, @Name, @IsCompleted, @CreatedAt, @UpdatedAt);";

            var now = DateTime.Now;
            var id = Guid.NewGuid();

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Guid);
            parameters.Add("Name", task.Name, DbType.String);
            parameters.Add("IsCompleted", task.IsCompleted, DbType.Boolean);
            parameters.Add("CreatedAt", now, DbType.DateTime2);
            parameters.Add("UpdatedAt", now, DbType.DateTime2);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);

                var createdTask = new TodoTask()
                {
                    Id = id,
                    Name = task.Name,
                    IsCompleted = task.IsCompleted,
                    CreatedAt = now,
                    UpdatedAt = now
                };

                return createdTask;
            }
        }

        public async Task UpdateTask(Guid id, UpdateTaskDto task)
        {
            var query = "UPDATE [dbo].[TodoTasks] SET Name = @Name, IsCompleted = @IsCompleted, UpdatedAt = @UpdatedAt WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Guid);
            parameters.Add("Name", task.Name, DbType.String);
            parameters.Add("IsCompleted", task.IsCompleted, DbType.Boolean);
            parameters.Add("UpdatedAt", DateTime.UtcNow, DbType.DateTime2);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task DeleteTask(Guid id)
        {
            var query = "DELETE FROM [dbo].[TodoTasks] WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Guid);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}
