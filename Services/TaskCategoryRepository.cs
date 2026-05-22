using KiwiTaskAPI.Database;
using KiwiTaskAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace KiwiTaskAPI.Services
{
    public class TaskCategoryRepository : ITaskCategoryRepository
    {
        private readonly AppDbContext _context;

        public TaskCategoryRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<TaskCategory>> GetTaskCategoriesAsync()
        {
            return await _context.task_categories.ToListAsync();
        }

        
    }
}
