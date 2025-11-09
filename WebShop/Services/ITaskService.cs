using System.Collections.Generic;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop.Services
{
    public interface ITaskService
    {
        Task<TaskItem> CreateAsync(TaskItem task);
        Task<TaskItem> GetAsync(int taskId);
        Task<IEnumerable<TaskItem>> GetByAssigneeAsync(string assigneeId);
        Task<TaskItem> UpdateAsync(TaskItem task);
        Task DeleteAsync(int taskId);
    }
}