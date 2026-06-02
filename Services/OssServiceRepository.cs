using Aliyun.OSS;
using KiwiTaskAPI.Options;
using Microsoft.Extensions.Options;



namespace KiwiTaskAPI.Services
{
    public class OssServiceRepository : IOssService
    {
        private readonly OssClient _client;
        private readonly OssOptions _opt;
        private readonly ILogger<OssServiceRepository> _logger;
        private static readonly HashSet<string> ImgExt = ["jpg", "jpeg", "png", "gif"];
        private static readonly HashSet<string> CvExt = ["pdf", "doc", "docx"];

        public OssServiceRepository(IOptions<OssOptions> opt, ILogger<OssServiceRepository> logger)
        {
            _opt = opt.Value;
            _client = new OssClient(_opt.Endpoint, _opt.AccessKey, _opt.AccessKeySecret);
            _logger = logger;
        }

        public async Task<string> UploadAsync(Stream stream, string key, string content_type)
        {
            var meta = new ObjectMetadata { ContentType = content_type };
            await Task.Run(() => _client.PutObject(_opt.Bucket, key, stream, meta));
            return $"{_opt.CdnUrl}{key}";
        }

        public async Task<string> UploadAvatarAsync(Stream stream, string file_name)
        {
            var ext = ValidateExt(file_name, ImgExt);
            var key = $"jobi/user-avatars/{Guid.NewGuid():N}.{ext}";
            return await UploadAsync(stream, key, $"image/{ext}");
        }

        private static string ValidateExt(string fileName, HashSet<string> allow)
        {
            var ext = Path.GetExtension(fileName).Trim('.').ToLowerInvariant();
            if (!allow.Contains(ext))
            {
                throw new InvalidOperationException($"Unsupported file type: {ext}");
            }
            return ext;
        }

    }
}
