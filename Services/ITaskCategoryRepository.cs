using KiwiTaskAPI.Models;

namespace KiwiTaskAPI.Services
{
    public interface ITaskCategoryRepository
    {
        // get all task categories
        Task<IEnumerable<TaskCategory>> GetTaskCategoriesAsync();
        
    }
}
