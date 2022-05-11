using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.IO;
using System.Threading.Tasks;
using Utilities.Models.Emails;

namespace Utilities.Emails
{
    public class SendMailService : ISendMailService
    {
        private readonly IConfiguration _configuration;

        public string mailConfigPort = string.Empty;
        public string mailConfigHost = string.Empty;
        public string mailConfigFrom = string.Empty;
        public string mailConfigPassword = string.Empty;

        public void LoadMailConfiguration()
        {
            mailConfigPort = _configuration.GetSection("EmailSettings:Port").Value;
            mailConfigHost = _configuration.GetSection("EmailSettings:Host").Value;
            mailConfigFrom = _configuration.GetSection("EmailSettings:Email").Value;
            mailConfigPassword = _configuration.GetSection("EmailSettings:Password").Value;
        }

        public SendMailService(IConfiguration configuration)
        {
            _configuration = configuration;
            LoadMailConfiguration();
        }

        public async Task<bool> SendEmail(EmailRequest emailRequest, string nameTemplate)
        {
            try
            {
                int port = int.Parse(mailConfigPort);
                string filePath = Directory.GetCurrentDirectory() + "\\Templates\\" + nameTemplate + ".html";
                StreamReader stream = new StreamReader(filePath);
                string mailText = stream.ReadToEnd();
                stream.Close();

                mailText = mailText.Replace("[Content]", emailRequest.Body);
                var email = new MimeMessage();

                email.Sender = MailboxAddress.Parse(mailConfigFrom);
                email.To.Add(MailboxAddress.Parse(emailRequest.ToEmail));

                if (!string.IsNullOrEmpty(emailRequest.CcEmail))
                {
                    email.Cc.Add(MailboxAddress.Parse(emailRequest.CcEmail));
                }

                if (!string.IsNullOrEmpty(emailRequest.BccEmail))
                {
                    email.Bcc.Add(MailboxAddress.Parse(emailRequest.BccEmail));
                }

                email.Subject = $"Người gửi: { emailRequest.Subject}";

                var builder = new BodyBuilder();

                if (emailRequest.Attachments != null)
                {
                    byte[] fileBytes;
                    foreach (var file in emailRequest.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                fileBytes = ms.ToArray();
                            }
                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                        }
                    }
                }

                builder.HtmlBody = mailText;
                email.Body = builder.ToMessageBody();

                using var smpt = new SmtpClient();
                smpt.Connect(mailConfigHost, port, SecureSocketOptions.StartTls);
                smpt.Authenticate(mailConfigFrom, mailConfigPassword);
                await smpt.SendAsync(email);
                smpt.Disconnect(true);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}