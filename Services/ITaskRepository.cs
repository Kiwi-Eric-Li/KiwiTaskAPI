using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Models;

namespace KiwiTaskAPI.Services
{
    public interface ITaskRepository
    {
        // 返回所有的任务
        Task<IEnumerable<Tasks>> GetTasksAsync();
        // 根据任务id，返回单个任务
        Task<Tasks> GetTaskByIdAsync(Guid taskId);
        Task<int> CreateTaskAsync(Tasks taskEntity);

        
    }
}
