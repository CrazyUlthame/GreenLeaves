using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.IO;
using System.Threading.Tasks;
using WebAPI.Models.Mail;
using WebAPI.Models.Request;
using WebAPI.Models.Response;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly Http _myService;
        private readonly IConfiguration _configuration;
        private readonly MailService _mailService;
        private readonly MailSettings _mailSettings;
        public MailSettings mailSettings { get; private set; }
        public UserController(Http myService, IOptions<MailSettings> mailsetting)
        {
            _myService = myService;
            mailSettings = new MailSettings();
            _mailSettings = mailsetting.Value;
            _mailService = new MailService();
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }


        // POST: UserController/Create
        [HttpPost]
        public STRResponse Create(UserModel Data)
        {
            var response = new STRResponse();
            var User = new UserService();
            if (ValidateDate(Data.Date))
            {
                try
                {
                    response = User.User(Data);
                }
                catch (Exception ex)
                {
                    response.IsError = true;
                    response.Message = ex.Message;
                }
                try
                {
                    int msg = 0;
                    var mail = new MailRequest();
                    do {
                        mail.Body = msg == 0 ? $"<!doctype html>\r\n<html lang=\"en\">\r\n<head>\r\n  <meta charset=\"utf-8\">\r\n  <title>Green Leaves</title>\r\n  <base href=\"/\">\r\n</head>\r\n    <body>\r\n        <div style=\"width: 50%;margin: 0 auto;\">\r\n            <div> \r\n                <label for=\"\">Estimado</label> <label style=\"font-weight: bold;\">{Data.Name},</label> \r\n            </div>\r\n            <br>\r\n            <div>\r\n                <label for=\"\">\r\n                    Hemos recibido sus datos y nos pondremos en contacto con usted en la \r\n                    brevedad posible. Enviaremos un correo con información a su cuenta: \r\n                </label>\r\n                <label style=\"font-weight: bold;\">{Data.Email}.</label>\r\n            </div>\r\n            <br>\r\n            <div style=\"text-align: right;\">\r\n                <p style=\"font-weight: bold;margin: 0;\">Atte.</p>\r\n                <p style=\"font-weight: bold; color: green;margin: 0;\">Green Leaves</p>\r\n                <p style=\"margin: 0;\">{Data.City + "," + Data.State + "," + Data.Country} a {Data.Date}</p>\r\n            </div>\r\n        </div>  \r\n    </body>\r\n</html>" : "hola mundo";
                        var toMAilJson = _mailSettings.Send.Mail;
                        mail.ToEmail = msg == 0 ? Data.Email : toMAilJson;
                        mail.Subject = "Confirmación de datos registrados.";
                        var resp = SendMailConfirmation(mail);
                        resp.Wait();
                        msg++;
                    } while (msg <= 1); 
                }
                catch (Exception ex)
                {
                    response.IsError = true;
                    response.Message = ex.Message;
                }
            }
            else {
                response.IsError = true;
                response.Message = "La fecha deberá estar en un rango entre hoy y hace 100 años. ";
            }
            

            return response;
        }

        private async Task<IActionResult> SendMailConfirmation(MailRequest request)
        {
            var response = await _myService.PostDataAsync("https://localhost:44378/api/Mail/SendMailConfirmation2", request);
            return Ok(response); // Retorna la respuesta del servicio
        }

        private bool ValidateDate(DateTime date)
        {
            var resp = true;
            try
            {
                DateTime hoy = DateTime.Today;
                DateTime haceCienAnios = hoy.AddYears(-100);

                if (date < haceCienAnios || date > hoy)
                {
                    resp = false;
                }
            }
            catch
            {
                resp = false;
            }            
            return resp;
        }
        
    }
}
