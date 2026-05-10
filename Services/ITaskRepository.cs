using KiwiTaskAPI.Models;

namespace KiwiTaskAPI.Services
{
    public interface ITaskRepository
    {
        // 返回所有的任务
        IEnumerable<Tasks> GetTasks();
        // 根据任务id，返回单个任务
        Tasks GetTaskById(Guid taskId);
    }
}
