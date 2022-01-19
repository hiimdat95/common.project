using cm.Utilities.Models.Emails;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cm.Utilities.Emails
{
    public interface ISendMailService
    {
        Task<bool> SendEmail(EmailRequest emailRequest, string nameTemplate);
        //bool SendMail(string toEmail, string ccEmail, string bccEmail, string title, string body, string template, Dictionary<string, byte[]> filePath);
    }
}
