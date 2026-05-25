using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Models;

namespace KiwiTaskAPI.Services
{
    public interface ITaskRepository
    {
        // get all tasks
        Task<(IEnumerable<Tasks>, int totalCount)> GetTasksAsync(int page_num, int page_size, string? title);
        // get the first few tasks
        Task<IEnumerable<Tasks>> GetFewTasksAsync();

        // get a task by task_id
        Task<Tasks> GetTaskByIdAsync(Guid taskId);
        Task<int> CreateTaskAsync(Tasks taskEntity);

        
    }
}
