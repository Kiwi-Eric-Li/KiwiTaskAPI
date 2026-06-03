


using KiwiTaskAPI.Database;
using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
            return await _context.tasks.Include(t => t.categories).Include(p => p.poster).FirstOrDefaultAsync(n => n.id == taskId);
        }

        public async Task<(IEnumerable<Tasks>, int totalCount)> GetTasksAsync(int page_num, int page_size, string? title)
        {
            var query = _context.tasks.Include(t => t.categories).Include(u => u.poster).AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(t => t.title.Contains(title));
            }
            var totalCount = await query.CountAsync();

            var tasks = await query.OrderByDescending(t => t.created_at)
                                .Skip((page_num - 1) * page_size)
                                .Take(page_size).ToListAsync();

            return (tasks, totalCount);
        }
        public async Task<IEnumerable<Tasks>> GetFewTasksAsync()
        {
            return await _context.tasks.OrderByDescending(t => t.created_at).Take(6).ToListAsync();
        }


        public async Task<int> CreateTaskAsync(Tasks taskEntity)
        {
            if(taskEntity.categories.Count > 0)
            {
                foreach(var item in taskEntity.categories)
                {
                    item.task_id = taskEntity.id;
                }
            }

            taskEntity.created_at = DateTime.Now;
            taskEntity.updated_at = DateTime.Now;
            taskEntity.status = "Open";

            using var transaction = await _context.Database.BeginTransactionAsync();
            // 1. save task
            await _context.tasks.AddAsync(taskEntity);

            // 2. save category
            await _context.task_cates.AddRangeAsync(taskEntity.categories);
            
            int result = await _context.SaveChangesAsync();
            // 3. commit transaction
            await transaction.CommitAsync();

            return result;
        }
    }
}
