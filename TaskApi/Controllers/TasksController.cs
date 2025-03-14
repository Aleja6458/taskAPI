using Microsoft.AspNetCore.Mvc;
using TaskApi.Data;
using TaskApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace TaskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskContext _context;

        public TasksController(TaskContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TaskItem>> GetTasks()
        {
            return _context.TaskItems.ToList();
        }

        [HttpGet("filter")]
        public ActionResult<IEnumerable<TaskItem>> GetFilteredTasks(bool isComplete)
        {
            var tasks = _context.TaskItems.Where(t => t.IsComplete == isComplete).ToList();
            return Ok(tasks);
        }

        [HttpPost]
        public ActionResult<TaskItem> CreateTask(TaskItem task)
        {
            _context.TaskItems.Add(task);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, TaskItem task)
        {
            var existingTask = _context.TaskItems.Find(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            existingTask.Name = task.Name;
            existingTask.IsComplete = task.IsComplete;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var task = _context.TaskItems.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.TaskItems.Remove(task);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
