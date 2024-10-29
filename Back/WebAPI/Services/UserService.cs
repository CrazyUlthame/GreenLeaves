using System;
using WebAPI.BD;
using WebAPI.Models.Response;


namespace WebAPI.Services
{
    public class UserService
    {
        public STRResponse User(WebAPI.Models.Request.UserModel Data){
            var response = new STRResponse();
            try
            {
                using (var context = new AplicacionDBContext())
                {
                    var nuevoUser = new WebAPI.Models.STR.Usuario.UserModel()
                    { 
                        Name = Data.Name, 
                      Email = Data.Email,
                      Phone = Data.Phone,
                      Date = Data.Date,
                      City  = Data.City,
                      State = Data.State,
                      Country = Data.Country};
                    context.Usuario.Add(nuevoUser);
                    context.SaveChanges();
                }
                response.IsError = false;
                response.Message = "Información Registrada Correctamente";
            }
            catch(Exception ex)
            {
                response.IsError = true;
                response.Message = ex.Message;
            }

            return response;
        
        }
    }
}
