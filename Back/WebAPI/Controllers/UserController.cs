using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Threading.Tasks;
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
        public UserController(Http myService)
        {
            _myService = myService;
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
                    var resp = SendMailConfirmation(Data);
                    resp.Wait();
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

        private async Task<IActionResult> SendMailConfirmation(UserModel request)
        {
            var response = await _myService.PostDataAsync("https://localhost:44378/api/Mail/SendMailConfirmation", request);
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
