using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using Org.BouncyCastle.Math.Field;
using System;
using System.IO;
using System.Linq.Expressions;
using WebAPI.Models.Mail;
using WebAPI.Models.Request;
using WebAPI.Models.Response;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MailService _mailService;
        private readonly MailSettings _mailSettings;
        public MailSettings mailSettings { get; private set; }

        public MailController(IOptions<MailSettings> mailsetting)
        {
            mailSettings = new MailSettings();
            _mailSettings = mailsetting.Value;
            _mailService = new MailService();
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        [HttpPost("SendMailConfirmation")]
        public IActionResult SendMailConfirmation([FromBody] UserModel email)
        {
            var res = new STRResponse();
            var mail = new MailService();
            int msg = 0;
            do
            {
                var body = msg == 0 ? $"<!doctype html>\r\n<html lang=\"en\">\r\n<head>\r\n  <meta charset=\"utf-8\">\r\n  <title>Green Leaves</title>\r\n  <base href=\"/\">\r\n</head>\r\n    <body>\r\n        <div style=\"width: 50%;margin: 0 auto;\">\r\n            <div> \r\n                <label for=\"\">Estimado</label> <label style=\"font-weight: bold;\">{email.Name},</label> \r\n            </div>\r\n            <br>\r\n            <div>\r\n                <label for=\"\">\r\n                    Hemos recibido sus datos y nos pondremos en contacto con usted en la \r\n                    brevedad posible. Enviaremos un correo con información a su cuenta: \r\n                </label>\r\n                <label style=\"font-weight: bold;\">{email.Email}.</label>\r\n            </div>\r\n            <br>\r\n            <div style=\"text-align: right;\">\r\n                <p style=\"font-weight: bold;margin: 0;\">Atte.</p>\r\n                <p style=\"font-weight: bold; color: green;margin: 0;\">Green Leaves</p>\r\n                <p style=\"margin: 0;\">{email.City + "," + email.State + "," + email.Country} a {email.Date}</p>\r\n            </div>\r\n        </div>  \r\n    </body>\r\n</html>" : "hola mundo";
                var toMAilJson = _mailSettings.Send.Mail;
                var toMail = msg == 0 ? email.Email : toMAilJson;
                msg++;
                try
                {
                    res = mail.SendEmail(toMail, "TestSubject", body, _mailSettings);
                    var ok = res.IsError == true ? true : false;
                    if (ok)
                        return BadRequest(res);

                }
                catch (Exception ex)
                {
                    res.IsError = true;
                    res.Message = ex.Message;
                    return BadRequest(res);
                }

            } while (msg <= 1);

            return Ok(mail);


        }

        [HttpPost("SendMailConfirmation2")]
        public IActionResult SendMailConfirmation2([FromBody] MailRequest email)
        {
            var res = new STRResponse();
            var mail = new MailService();
            try
            {
                res = mail.SendEmail(email.ToEmail, email.Subject, email.Body, _mailSettings);
                var ok = res.IsError == true ? true : false;
                if (ok)
                    return BadRequest(res);

            }
            catch (Exception ex)
            {
                res.IsError = true;
                res.Message = ex.Message;
                return BadRequest(res);
            }

            return Ok(res);


        }
    }
}
