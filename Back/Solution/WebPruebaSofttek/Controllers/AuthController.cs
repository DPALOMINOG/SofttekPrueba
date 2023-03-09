using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Dynamic;
using WebPruebaSofttek.Models.RequestBE;
using WebPruebaSofttek.Models.ResponseBE;
using WebPruebaSofttek.Service;

namespace WebPruebaSofttek.Controllers
{
    #region "AUT" 
    [ApiVersion("1")]
    [Route("api/recaudo/v{version:apiVersion}/autenticacion")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IConfiguration Configuration { get; set; }
        private IUserService _userService;

        #region "CONFIGURACION"
        public AuthController(IUserService userService, IConfiguration configuration)
        {
            Configuration = configuration;
            _userService = userService;
        }
        #endregion
        #region "POST"
        [HttpPost]
        public IActionResult Authenticate([FromBody]AuthCompanyBE model)
        {
            DateTime FechaHoy = DateTime.Now;
            DateTime Fecha_CapturaTransasaccion = new DateTime(FechaHoy.Year, FechaHoy.Month, FechaHoy.Day, FechaHoy.Hour, FechaHoy.Minute, FechaHoy.Second);
            var formatoo = Fecha_CapturaTransasaccion;
            var formatoo2 = Fecha_CapturaTransasaccion.ToString("yyyy-MM-ddTHH:mm:ss");
            //TODO: Prueba SCO 12102021
            //var PRUEBA = "1452254154;A15B148;GVGFDFG{'FFG':'GDFGFDG'}";
            //string[] stringSeparators = new string[] { ";" }; 
            //char[] charSeparators = new char[] { ';' };
            //var result22 = PRUEBA.Split(charSeparators, StringSplitOptions.None);
            //var prueba2 = result22.Length > 0 ? result22[0].ToString() : "";
            ////////////////////////////


            var _RESPONSE = _userService.Authenticate(model);
            var codigoRespuesta = _RESPONSE.codigoRespuesta;
            var mensaje = Configuration.GetValue<string>("CodigoErrors:" + codigoRespuesta);
            if (codigoRespuesta == "00")
            {
                _RESPONSE.mensaje = mensaje;
                var val = JsonConvert.SerializeObject(_RESPONSE, Formatting.Indented);
                var converter = new ExpandoObjectConverter();
                dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(val, converter);
                return Ok(result);
            }
            else
            {
                var _base = new BaseResponse();
                _base.codigoRespuesta = codigoRespuesta;
                _base.mensaje = mensaje != null ? mensaje : _RESPONSE.mensaje.Trim();
                return Ok(_base);
            }
        }
        #endregion
    }
    #endregion
}