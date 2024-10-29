using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.IO;
using System.Threading.Tasks;
using WebAPI.Models.Mail;
using WebAPI.Models.Response;

namespace WebAPI.Services
{    

    public class MailService
    {
        
        public STRResponse SendEmail(string mail, string subject  ,string body, MailSettings mailSetting)
        {
            var response = new STRResponse();
            using var client = new SmtpClient();
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(mailSetting.Settings.Name, mailSetting.Settings.From));
                message.To.Add(new MailboxAddress("", mail));
                message.Subject = subject;
                message.Body = new TextPart("html") { Text = body };
                
                client.Connect("smtp.gmail.com", 587);
                client.Authenticate(mailSetting.Settings.From, mailSetting.Settings.Pass);
                client.Send(message);                

                response.IsError = false;
                response.Message = "OK";
            }
            catch(Exception ex)
            {
                response.IsError = true;
                response.Message = ex.Message;
            }
            finally
            {
                client.Disconnect(true);
            }
            return response;
        }
    }
        
    
}
