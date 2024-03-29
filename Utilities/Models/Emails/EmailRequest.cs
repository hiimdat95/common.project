﻿using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Utilities.Models.Emails
{
    public class EmailRequest
    {
        public string ToEmail { get; set; }
        public string CcEmail { get; set; }
        public string BccEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}