


using KiwiTaskAPI.Database;
using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace KiwiTaskAPI.Services
{
    public class TaskRepository : ITaskRepository
    {
        // private List<Tasks> _tasks;    // mock数据
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<Tasks> GetTaskByIdAsync(Guid taskId)
        {
            return await _context.tasks.FirstOrDefaultAsync(n => n.id == taskId);
        }

        public async Task<IEnumerable<Tasks>> GetTasksAsync()
        {
            return await _context.tasks.ToListAsync();
        }

        public async Task<int> CreateTaskAsync(Guid poster_id, TasksDto taskDto)
        {
            return 1;
        }
    }
}
