
using KiwiTaskAPI.Database;
using KiwiTaskAPI.Hubs;
using KiwiTaskAPI.Models;
using KiwiTaskAPI.Types;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KiwiTaskAPI.Services
{
    public class TaskNotificationServiceRepository : ITaskNotificationService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<TaskNotificationsHub> _hub;

        public TaskNotificationServiceRepository(AppDbContext context, IHubContext<TaskNotificationsHub> hub)
        {
            _context = context;
            _hub = hub;
        }


        public async Task<int> GetUnReadNotificationCountAsync(Guid userId)
        {
            return await _context.task_notifications.Where(x => x.user_id == userId && x.is_read == 0).CountAsync();
        }

        public async Task PushAsync(Guid userId, string type, string title, string body, Guid? taskId, int? offerId)
        {
            var entity = new TaskNotifications
            {
                user_id = userId,
                type = type,
                title = title,
                body = body,
                task_id = taskId,
                offer_id = offerId,
                is_read = 0,
                created_at = DateTime.UtcNow,
                read_at = null
            };

            _context.task_notifications.Add(entity);
            await _context.SaveChangesAsync();

            await _hub.Clients.Group($"user:{userId}").SendAsync(HubEvents.Notifiy, new {
                id = entity.id,
                type = entity.type,
                title = entity.title,
                body = entity.body,
                task_id = entity.task_id,
                offer_id = entity.offer_id,
                is_read = 0,
                created_time = DateTime.UtcNow
            });

        }
    }
}
