using KiwiTaskAPI.Database;
using KiwiTaskAPI.Dtos;

namespace KiwiTaskAPI.Services
{
    public class TaskMediaServiceRepository : ITaskMediaService
    {
        private readonly AppDbContext _context;


        public TaskMediaServiceRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> ConfirmAsync(Guid uploader_id, TaskMediaConfirmDto dto)
        {
            return true;
        }

        public async Task<string> UploadAsync(Guid uploader_id, IFormFile file)
        {
            return "";
        }
    }
}
