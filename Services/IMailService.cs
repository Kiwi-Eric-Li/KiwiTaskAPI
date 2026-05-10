namespace KiwiTaskAPI.Services
{
    public interface IMailService
    {
        Task SendWelcomeEmailAsync(string name, string recipient);
    }
}
