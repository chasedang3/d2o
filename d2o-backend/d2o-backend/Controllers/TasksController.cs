using Microsoft.AspNetCore.Mvc;
using d2o_backend.Models;
using d2o_backend.Repositories;

namespace d2o_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasksRepository _tasksRepository;

        public TasksController(ITasksRepository tasksRepository)
        {
            _tasksRepository = tasksRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var todoTasks = await _tasksRepository.GetAllTasks();
                return Ok(todoTasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(AddTaskDto task)
        {
            try
            {
                var createdTask = await _tasksRepository.AddTask(task);
                return Ok(createdTask);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateTask(Guid id, UpdateTaskDto task)
        {
            try
            {
                var dbTask = await _tasksRepository.GetTaskById(id);
                if (dbTask == null)
                    return NotFound();

                await _tasksRepository.UpdateTask(id, task);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            try
            {
                var dbTask = await _tasksRepository.GetTaskById(id);
                if (dbTask == null)
                    return NotFound();

                await _tasksRepository.DeleteTask(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
