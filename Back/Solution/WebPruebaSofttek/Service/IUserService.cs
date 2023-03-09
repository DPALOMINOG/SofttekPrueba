using Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebPruebaSofttek.Models;
using WebPruebaSofttek.Models.RequestBE;
using WebPruebaSofttek.Models.ResponseBE;

namespace WebPruebaSofttek.Service
{
    public interface IUserService
    {
        Auth Authenticate(AuthCompanyBE Auth);
        IEnumerable<Auth> GetAll();
    }


    public class ServiceUser : IUserService
    {
        /*****************************/
        public Int32 EmpresaId { get; set; }
        public String CodigoEmpresa { get; set; } = "00";
        public Int32 APICredencialId { get; set; } = 0;
        String fecha;
        private CoreDBContext context = null;
        /*****************************/
        public IConfiguration Configuration { get; set; }
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<Auth> _users = new List<Auth>();
        private readonly AppSettings _appSettings;
        /// <summary>
        /// ///////////////////
        /// </summary>  
        JwtSecurityTokenHandler handler;
        public ServiceUser(IConfiguration configuration, IOptions<AppSettings> appSettings)
        {
            handler = new JwtSecurityTokenHandler();
            Configuration = configuration;
            _appSettings = appSettings.Value;
            fecha = DateTime.Now.Date.ToString("yyyy-MM-dd");
            var Connect = Configuration.GetConnectionString("STKDataBase");
            context = new CoreDBContext(Connect);
        }

        public  Auth Authenticate(AuthCompanyBE Auth)
        {
            var Autenticar = new Auth();
            Autenticar.mensaje = "Error!";
            Autenticar.codigoRespuesta = "99";
            try
            {
                //DANIEL
                //PEPE
                //PALOMINO
                Auth.usuario = /*Encriptacion.Encriptar(*/Auth.usuario.ToString()/*, Configuration);*/;
                Auth.clave = /*Encriptacion.Encriptar(*/Auth.clave.ToString()/*, Configuration);*/;
                #region "GENERAR TOKEN" 

                var tokenString = String.Empty;// SE PUEDE MEJORAR
                if (Auth.usuario == "PEPE")
                {
                    Autenticar.codigoRespuesta = "02";
                    Autenticar.mensaje = "Usuario Vencido!";
                    return Autenticar;
                }
                if (Auth.usuario == "PALOMINO")
                {
                    Autenticar.codigoRespuesta = "03";
                    Autenticar.mensaje = "Usuario No existe!";
                    return Autenticar;
                }
                if (Auth.usuario == "DANIEL")
                {
                    Autenticar.codigoRespuesta = "00";
                    Autenticar.mensaje = "Ok!"; 
                }

                // authentication successful so generate jwt token 
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

                var _fechafin = fecha + " 23:59:59";
                var fechafin = DateTime.Parse(_fechafin);
                var tokenDescriptor = new SecurityTokenDescriptor();
                ///////
                var _n = System.Guid.NewGuid().ToString("N").Substring(1, 6);
                string Hast = Auth.usuario + "," + Auth.clave + "," + _n;// se puede agregar encriptacion
                tokenDescriptor.Subject = new ClaimsIdentity(new Claim[]
                    {// SE PUEDE MEJORAR
                        new Claim("Hast", Hast),
                        new Claim("Fecha",fecha),
                        new Claim("API", _n)
                    });
                tokenDescriptor.Expires = fechafin.AddDays(10);// DateTime.Now.AddDays(1),
                tokenDescriptor.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
                ///////
                var timeSpan = (fechafin - DateTime.Now);
                var timeMinuts = (timeSpan.Hours * 60) + timeSpan.Minutes;
                Autenticar.expira_en = "" + timeMinuts;

                var _token = tokenHandler.CreateToken(tokenDescriptor);
                var stringtoken = tokenHandler.WriteToken(_token);
                Autenticar.token = stringtoken;
                Autenticar.refresh_token = stringtoken;

                #endregion

                //context.SaveChanges();
            }
            catch (Exception)
            {
                Autenticar.codigoRespuesta = "99";
            }


            return Autenticar;
        }
 
        public IEnumerable<Auth> GetAll()
        {
            return _users;
        }


    }
}
