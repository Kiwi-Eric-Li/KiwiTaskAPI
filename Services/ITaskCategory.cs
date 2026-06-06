using KiwiTaskAPI.Models;

namespace KiwiTaskAPI.Services
{
    public interface ITaskCategory
    {
        // get all task categories
        Task<IEnumerable<TaskCategory>> GetTaskCategoriesAsync();
        
    }
}
