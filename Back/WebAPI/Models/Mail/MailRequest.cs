using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WebAPI.Models.Mail
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }

    public class MailSettings
    {
        public Send Send { get; set; }
        public Settings Settings { get; set; }
    }


    public class Send
    {
        public string Mail { get; set; }
    }

    public class Settings
    {
        public string From { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
    }
}
