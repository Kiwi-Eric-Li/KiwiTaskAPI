using System.ComponentModel.DataAnnotations;

namespace KiwiTaskAPI.Options
{
    public record OssOptions
    {

        [Required] 
        public string AccessKey { get; init; } = default!;
        [Required] 
        public string AccessKeySecret { get; init; } = default!;
        [Required]
        public string Endpoint { get; init; } = default!;
        [Required]
        public string Bucket { get; init; } = default!;
        [Required]
        public string CdnUrl { get; init; } = default!;

    }
}
