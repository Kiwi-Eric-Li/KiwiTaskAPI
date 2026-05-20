using KiwiTaskAPI.Dtos;

namespace KiwiTaskAPI.Services
{
    public interface ITaskMediaService
    {
        Task<string> UploadAsync(Guid uploader_id, IFormFile file);
        Task<bool> ConfirmAsync(Guid uploader_id, TaskMediaConfirmDto dto);
    }
}
