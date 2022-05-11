using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Utilities.Common;

namespace Application.Services
{
    public interface IEmailService
    {
        void Send(string to, string subject, string html, string from = null);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<SmtpConfig> _smtpConfig;

        public EmailService(IConfiguration configuration, IOptions<SmtpConfig> smtpConfig)
        {
            _configuration = configuration;
            _smtpConfig = smtpConfig;
        }

        public void Send(string to, string subject, string html, string from = null)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? _smtpConfig.Value.EmailFrom));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_smtpConfig.Value.SmtpHost, _smtpConfig.Value.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_smtpConfig.Value.SmtpUser, _smtpConfig.Value.SmtpPass);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}