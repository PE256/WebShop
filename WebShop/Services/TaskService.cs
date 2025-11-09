using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Services
{
    public class TaskService : ITaskService
    {
        private readonly DataContext _context;

        public TaskService(DataContext context)
        {
            _context = context;
        }

        public async Task<TaskItem> CreateAsync(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task DeleteAsync(int taskId)
        {
            var t = await _context.Tasks.FindAsync(taskId);
            if (t != null)
            {
                _context.Tasks.Remove(t);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TaskItem> GetAsync(int taskId)
        {
            return await _context.Tasks
                .Include(t => t.Assignee)
                .Include(t => t.Document)
                .Include(t => t.CreatedByUser)
                .SingleOrDefaultAsync(t => t.TaskId == taskId);
        }

        public async Task<IEnumerable<TaskItem>> GetByAssigneeAsync(string assigneeId)
        {
            return await _context.Tasks
                .Where(t => t.AssigneeId == assigneeId)
                .Include(t => t.Document)
                .ToListAsync();
        }

        public async Task<TaskItem> UpdateAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }
    }
}