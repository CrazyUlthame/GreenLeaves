using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WebAPI.BD;
using WebAPI.Models.Response;

namespace WebAPI.Services
{
    public class LocalidadesService
    {
        public STRResponseList  getLocalidadesPorCoincidencia(string str)
        {
            var response = new STRResponseList();

            try
            {
                using (var context = new AplicacionDBContext())
                {
                    var localidades = context.Localidades
                        .Where(p => EF.Functions.Like(p.CIudad, $"%{str}%") ||
                                    EF.Functions.Like(p.Estado, $"%{str}%"))
                        .ToList();
                    response.List = localidades.Cast<object>().ToList();
                }

                response.IsError = false;
                response.Message = "Ok";
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.Message = ex.Message;
            }


            return response;
        }
    }
}
