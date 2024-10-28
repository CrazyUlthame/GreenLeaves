using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WebAPI.Models.Response;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalidadesController : ControllerBase
    {
        [HttpGet]
        public STRResponseList GetLocalidades(string str) { 
            var response = new STRResponseList();
            var localidad = new LocalidadesService();
            try
            {
                response = localidad.getLocalidadesPorCoincidencia(str);
            }
            catch (Exception ex) { 
                response.IsError = true; response.Message = ex.Message; 
            }
             


            return response;
        }
    }
}
