namespace KiwiTaskAPI.Options
{
    public record SmtpOptions
    {
        public string Host { get; init; } = "smtphz.qiye.163.com";
        public int Port { get; init; } = 465;
        public string HeaderImg { get; init; } = "https://liuxuebang.oss-ap-southeast-1.aliyuncs.com/kiwisquare/logo/kiwisquare_white.png";
        public string FooterImg { get; init; } = "https://liuxuebang.oss-ap-southeast-1.aliyuncs.com/kiwisquare/logo/kiwisquare_white.png";
        public string Company { get; init; } = "KiwiSquare";
        public string DefaultSender { get; init; } = "info@kiwisquare.co.nz";
        public bool UseSsl { get; init; } = true;
        public string User { get; init; } = default!;
        public string Password { get; init; } = default!;
    }
}
