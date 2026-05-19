


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
            //if(_tasks == null)
            //{
            //    InitializeTasks();
            //}
            _context = context;
        }

        //private void InitializeTasks()
        //{
        //    _tasks = new List<Tasks>
        //    {
        //        new Tasks
        //        {
        //            Id = Guid.NewGuid(),
        //            Title = "Replace loose roof tiles after recent wind",
        //            Description = "Hi, we had some strong winds last week and a few concrete roof tiles came loose on our single-storey house in Henderson. They're sliding around and I'm worried they might fall or cause leaks. I need someone to safely access the roof, secure the loose tiles properly, and check if any adjacent tiles were damaged. I have replacement tiles that match if any are broken. The roof pitch isn't too steep and I can provide a ladder. Please let me know if you have roofing experience and can do this safely."
        //        },
        //        new Tasks
        //        {
        //            Id = Guid.NewGuid(),
        //            Title = "Help sort and file paperwork at home office",
        //            Description = "I've let my home office paperwork pile up over the last few months and need help getting it organised. I have a mix of personal bills, receipts, and some light business documents that need sorting into categories, filing in the correct folders, and shredding anything that's no longer needed. I'll provide all the folders, labels, and a shredder. You should be organised, discreet with personal information, and able to work systematically. The room is clean and has a desk setup ready to go."
        //        },
        //        new Tasks
        //        {
        //            Id = Guid.NewGuid(),
        //            Title = "Trim overhanging branches from fence line",
        //            Description = "I have a large tree in my backyard with several branches hanging over my neighbour's fence. They're getting quite long and starting to scrape against their roof during windy days. Need someone to safely trim back about 4-5 branches that are extending 2-3 meters over the boundary. I've already spoken with the neighbour and they're happy for the work to be done. The tree is accessible from my property, and I can help with cleanup of smaller branches. Please bring your own tools and safety equipment. The branches are about 10-15cm thick at the base."
        //        },
        //        new Tasks
        //        {
        //            Id = Guid.NewGuid(),
        //            Title = "Car interior deep clean and vacuum",
        //            Description = "Hi there! I need someone to give my 2018 Toyota Corolla a thorough interior clean. I have two young kids and the car has accumulated crumbs, spills, and general mess in the back seats and footwells. I'd like all seats vacuumed (including under child seats), the mats shaken out and vacuumed, the dashboard and console wiped down, and the windows cleaned inside. I can provide a vacuum cleaner and cleaning products if needed, but you're welcome to bring your own. The car will be parked at my home in Newmarket. I'm flexible on the day, but would prefer a weekend morning."
        //        }
        //    };
        //}

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
