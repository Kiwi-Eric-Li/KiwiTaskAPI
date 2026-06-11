using KiwiTaskAPI.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace KiwiTaskAPI.Hubs
{
    public class TaskNotificationsHub : Hub
    {
        private readonly ILogger<TaskNotificationsHub> _log;
        public TaskNotificationsHub(ILogger<TaskNotificationsHub> log)
        {
            _log = log;
        }

        [Authorize]
        public async Task JoinedTask(Guid taskid)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, HubGroups.Task(taskid));
            _log.LogInformation("Conn {ConnId} joined {Group}", Context.ConnectionId, HubGroups.Task(taskid));
            await Clients.Caller.SendAsync(HubEvents.JoinedTask, new { task_id = taskid});
        }

        [Authorize]
        public async Task LeftTask(Guid taskid)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, HubGroups.Task(taskid));
            _log.LogInformation("Conn {ConnId} left {Group}", Context.ConnectionId, HubGroups.Task(taskid));
            await Clients.Caller.SendAsync(HubEvents.LeftTask, new { task_id = taskid });
        }


    }
}
