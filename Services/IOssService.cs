namespace KiwiTaskAPI.Services
{
    public interface IOssService
    {
        Task<string> UploadAsync(Stream stream, string key, string content_type);
        Task<string> UploadAvatarAsync(Stream stream, string file_name);
    }
}
