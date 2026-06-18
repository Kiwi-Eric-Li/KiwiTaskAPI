using KiwiTaskAPI.Models;
using KiwiTaskAPI.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KiwiTaskAPI.Hubs
{
    [Authorize]
    public class TaskNotificationsHub : Hub
    {
        private readonly ILogger<TaskNotificationsHub> _log;
        public TaskNotificationsHub(ILogger<TaskNotificationsHub> log)
        {
            _log = log;
        }

        public override async Task OnConnectedAsync()
        {

            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            _log.LogInformation("Connected userId={UserId}", userId);

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, HubGroups.User(Guid.Parse(userId)));
                _log.LogInformation("Add group={Group}", HubGroups.User(Guid.Parse(userId)));
            }

            Console.WriteLine($"Connected-id: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _log.LogInformation("Hub disconnected: conn={ConnId}", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinedTask(Guid taskid)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, HubGroups.Task(taskid));
            _log.LogInformation("Conn {ConnId} joined {Group}", Context.ConnectionId, HubGroups.Task(taskid));
            await Clients.Caller.SendAsync(HubEvents.JoinedTask, new { task_id = taskid});
        }

        public async Task LeftTask(Guid taskid)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, HubGroups.Task(taskid));
            _log.LogInformation("Conn {ConnId} left {Group}", Context.ConnectionId, HubGroups.Task(taskid));
            await Clients.Caller.SendAsync(HubEvents.LeftTask, new { task_id = taskid });
        }


    }
}
