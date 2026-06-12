using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Models;

namespace KiwiTaskAPI.Services
{
    public interface ITaskService
    {
        // get all tasks
        Task<(IEnumerable<TaskListDto>, int totalCount)> GetTasksAsync(int page_num, int page_size, string? title);
        // get the first few tasks
        Task<IEnumerable<Tasks>> GetFewTasksAsync();

        // get a task by task_id
        Task<TasksDto> GetTaskByIdAsync(Guid taskId);
        Task<int> CreateTaskAsync(Tasks taskEntity);

        // cancel a task
        Task<int> CancelTaskAsync(Guid taskId);

        
    }
}
