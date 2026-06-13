


using AutoMapper;
using KiwiTaskAPI.Database;
using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;

namespace KiwiTaskAPI.Services
{
    public class TaskServiceRepository : ITaskService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        
        public TaskServiceRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> CancelTaskAsync(Guid taskId)
        {
            var task = await _context.tasks.FirstOrDefaultAsync(t => t.id == taskId); 
            if(task == null)
            {
                return 0;
            }
            task.status = "Cancelled";
            return await _context.SaveChangesAsync();
        }

        public async Task<TasksDto> GetTaskByIdAsync(Guid taskId)
        {
            var task = await _context.tasks.Include(t => t.categories).Include(p => p.poster).Include(o => o.offers).ThenInclude(u => u.user).FirstOrDefaultAsync(n => n.id == taskId);
            if(task is not null)
            {
                task.offers = task.offers.GroupBy(o => o.user_id).Select(g => g.OrderByDescending(o => o.created_at).First()).ToList();
            }
            return _mapper.Map<TasksDto>(task);
        }

        public async Task<(IEnumerable<TaskListDto>, int totalCount)> GetTasksAsync(int page_num, int page_size, string? title)
        {
            var query = _context.tasks.AsQueryable();
            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(t => t.title.Contains(title));
            }
            var totalCount = await query.CountAsync();
            var tasks = await query.OrderByDescending(t => t.created_at).Where(t => t.status == "Open")
                    .Skip((page_num - 1) * page_size)
                    .Take(page_size).Select(t => new TaskListDto
                    {
                        id = t.id,
                        poster_id = t.poster_id,
                        title = t.title,
                        task_type = t.task_type,
                        pricing_type = t.pricing_type == 0 ? "Fixed" : "Hourly",
                        budget = t.budget,
                        estimated_hours = t.estimated_hours,
                        expires_at = t.expires_at,
                        location = t.location,
                        suburb = t.suburb,
                        city = t.city,
                        postcode = t.postcode,
                        latitude = t.latitude,
                        longitude = t.longitude,
                        schedule_time = t.schedule_time,
                        created_at = t.created_at,
                        status = t.status,
                        poster = new UsersDto
                        {
                            id = t.poster_id,
                            firstname = t.poster.firstname,
                            lastname = t.poster.lastname,
                            avatar_url = t.poster.avatar_url,
                            username = t.poster.username
                        },
                        categories = t.categories.Select(c => new TaskCatesDto
                        {
                            id = c.id,
                            title = c.title
                        }).ToList(),
                        offer_count = t.offers.Select(o => o.user_id).Distinct().Count(),
                        comment_count = 0
                    }).ToListAsync();
            
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
