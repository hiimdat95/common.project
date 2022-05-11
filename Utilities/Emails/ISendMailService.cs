using System.Threading.Tasks;
using Utilities.Models.Emails;

namespace Utilities.Emails
{
    public interface ISendMailService
    {
        Task<bool> SendEmail(EmailRequest emailRequest, string nameTemplate);

        //bool SendMail(string toEmail, string ccEmail, string bccEmail, string title, string body, string template, Dictionary<string, byte[]> filePath);
    }
}