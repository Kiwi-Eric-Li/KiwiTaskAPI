
using KiwiTaskAPI.Models;
using KiwiTaskAPI.Options;
using Microsoft.Extensions.Options;
using MimeKit;
using RazorLight;
using System.Net;
using System.Text;
using MailKit.Net.Smtp;
using MailKit.Security;


namespace KiwiTaskAPI.Services
{
    public class MailServiceRepository : IMailService
    {
        private readonly IConfiguration _configuration;
        private readonly SmtpOptions _opt;
        private readonly RazorLightEngine _razor;
        private readonly string _frontUrl;

        
        public MailServiceRepository(IOptions<SmtpOptions> opt, IConfiguration configuration)
        {
            _opt = opt.Value;
            _configuration = configuration;
            _razor = new RazorLightEngineBuilder().UseFileSystemProject(Path.Combine(Directory.GetCurrentDirectory(), "Templates")).UseMemoryCachingProvider().Build();
            _frontUrl = _configuration["FrontUrl"];
        }

        private async Task SendAsync(string to, string subject, string htmlBody)
        {
            var msg = new MimeMessage();
            msg.From.Add(MailboxAddress.Parse(_opt.DefaultSender));
            msg.To.Add(MailboxAddress.Parse(to));
            msg.Subject = subject;
            msg.Body = new TextPart("html") { Text = htmlBody };

            using var client = new SmtpClient();
            await client.ConnectAsync(
                _opt.Host,
                _opt.Port,
                _opt.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls
            );
            await client.AuthenticateAsync(_configuration["Smtp:User"], _configuration["Smtp:Password"]);
            await client.SendAsync(msg);
            await client.DisconnectAsync(true);
        }


        public async Task SendWelcomeEmailAsync(string name, string recipient)
        {
            var emailModel = new EmailModel
            {
                Name = name,
                CompanyName = _opt.Company,
                FrontUrl = _frontUrl,
                HeaderImage = _opt.HeaderImg,
                FooterImage = _opt.FooterImg
            };

            string html = string.Empty;
            html = await _razor.CompileRenderAsync("Emails/WelcomeEmail.cshtml", emailModel);
            await SendAsync(recipient,$"Welcome to {_opt.Company}" , html);
        }
    }
}
