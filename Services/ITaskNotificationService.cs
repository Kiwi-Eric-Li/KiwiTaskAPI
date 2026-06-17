namespace KiwiTaskAPI.Services
{
    public interface ITaskNotificationService
    {
        Task PushAsync(Guid userId, string type, string title, string body, Guid? taskId, int? offerId);

        Task<int> GetUnReadNotificationCountAsync(Guid userId);
    }
}
