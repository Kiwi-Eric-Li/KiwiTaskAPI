using KiwiTaskAPI.Models;

namespace KiwiTaskAPI.Services
{
    public interface ITaskCategoryRepository
    {
        // get all task categories
        Task<IEnumerable<TaskCategory>> GetTaskCategoriesAsync();
        // batch add task category
        Task<int> BatchAddTaskCategory(List<TaskCates> taskCatesList);
    }
}
