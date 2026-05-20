using Aliyun.OSS;
using KiwiTaskAPI.Options;
using Microsoft.Extensions.Options;



namespace KiwiTaskAPI.Services
{
    public class IOssServiceRepository : IOssService
    {
        private readonly OssClient _client;
        private readonly OssOptions _opt;
        private readonly ILogger<IOssServiceRepository> _logger;
        private static readonly HashSet<string> ImgExt = ["jpg", "jpeg", "png", "gif"];
        private static readonly HashSet<string> CvExt = ["pdf", "doc", "docx"];

        public IOssServiceRepository(IOptions<OssOptions> opt, ILogger<IOssServiceRepository> logger)
        {
            _opt = opt.Value;
            _client = new OssClient(_opt.Endpoint, _opt.AccessKey, _opt.AccessKeySecret);
            _logger = logger;
        }

        async Task<string> IOssService.UploadAsync(Stream stream, string key, string content_type)
        {
            var meta = new ObjectMetadata { ContentType = content_type };
            await Task.Run(() => _client.PutObject(_opt.Bucket, key, stream, meta));
            return $"{_opt.CdnUrl}{key}";
        }

        Task<string> IOssService.UploadAvatarAsync(Stream stream, string file_name)
        {
            throw new NotImplementedException();
        }
    }
}
