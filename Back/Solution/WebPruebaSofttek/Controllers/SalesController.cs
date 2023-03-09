using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebPruebaSofttek.Models;
using WebPruebaSofttek.Models.ResponseBE;
using WebPruebaSofttek.Service;

namespace WebPruebaSofttek.Controllers
{
    [ApiVersion("1")]
    [Route("api/recaudo/v{version:apiVersion}/Ventas")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        public static IConfiguration Configuration { get; set; }
        CoreDBContext context = null;
        // GET: Sales
        public SalesController(IConfiguration configuration)
        {
            Configuration = configuration;
            var Connect = Configuration.GetConnectionString("DataBase");
            context = new CoreDBContext(Connect);
        }
        #region "GET"
        [HttpGet]
        [Route("/consulta")]
        [Authorize]
        public IActionResult GetConsulta(String ProductoName)
        {
            var resultError = new BaseResponse();
            var result = new List<Tbsaleasecom>();
            var Manager = new ITokenService(Configuration);
            var token = HttpContext.GetTokenAsync("access_token");
            try
            {
                string _Token = token.Result.ToString();
                var FechaActual = DateTime.Now.ToString("yyyyMMdd");
                if (Manager.ValidarToken(_Token))
                {
                    if (string.IsNullOrEmpty(ProductoName))
                    {
                        var LisTotal = context.Tbsaleasecom.ToList(); 
                        return Ok(LisTotal);
                    }else//(ProductoName != "")
                    {
                        var LisTotal = context.Tbsaleasecom.Where(x => x.Productname.ToUpper() == ProductoName.ToUpper()).ToList(); 
                        return Ok(LisTotal);
                    } 
                     
                }
                else
                {
                    resultError.codigoRespuesta = "98";
                    resultError.mensaje = "Token Invalido";
                }
            }
            catch (Exception ex)
            {
                resultError.codigoRespuesta = "99";
                resultError.mensaje = ex.Message;
            }

            return Ok(resultError);
        }
        #endregion


    }
}