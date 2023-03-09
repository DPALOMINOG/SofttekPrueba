using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebPruebaSofttek.Models;

namespace WebPruebaSofttek.Service
{
    public class ITokenService
    {
        SymmetricSecurityKey securityKey;
        SigningCredentials credentials;
        JwtHeader header;
        JwtSecurityTokenHandler handler;
        CoreDBContext context;
        //
        String key;
        String fecha;
        public bool Error { get; set; } = false;
        public static IConfiguration Configuration { get; set; }
        public ITokenService(IConfiguration configuration)
        {
            Configuration = configuration;
            key = Configuration.GetValue<string>("Security:PrivateKeyJWT"); 
            securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            header = new JwtHeader(credentials);
            handler = new JwtSecurityTokenHandler();
            fecha = DateTime.Now.Date.ToString("yyyy-MM-dd");  
            context = new CoreDBContext(Configuration.GetConnectionString("DataBase"));
        }
        #region "ETHICAL"
        public  bool ValidarToken(String Token)
        {
            try
            {
                var token = handler.ReadJwtToken(Token);
                var lstClaims = new List<String>();
                foreach (var claim in token.Claims)
                {
                    lstClaims.Add(claim.Value);
                }
                if (lstClaims.Count == 6 && token.Header.Alg == "HS256" && token.Header.Typ == "JWT")
                {
                    var signature = HmacSha256Digest(token.RawHeader + "." + token.RawPayload, key);
                    signature = HexString2B64String(signature).Replace("+", "-").Replace("/", "_").Replace("=", String.Empty);
                    var Hast =lstClaims[0];
                    var FechaDescr = lstClaims[1];
                    var API = lstClaims[2];
                    string[] arr = Hast.Split(new string[] { "," }, StringSplitOptions.None);
                    if (signature == token.RawSignature && FechaDescr == fecha && arr.Length == 3)
                    {
                        var CodigoUsuario = arr[0].ToString();
                        var Password = arr[1].ToString();// SE PUEDE MEJORAR
                        if (CodigoUsuario == "PEPE") { return false; }
                        if (CodigoUsuario == "PALOMINO") { return false; }
                        //if (CodigoUsuario == "DANIEL") { return false; } 
                        return true;
                        //}
                    }
                    else if (signature == token.RawSignature && FechaDescr != fecha)//lstClaims[2] != fecha)
                    {
                        Error = true;
                    }
                }
                return false;
            }
            catch (Exception)// ex)
            {
                return false;
            }
        }
        #endregion

        public string HmacSha256Digest(string message, string secret)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] keyBytes = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            System.Security.Cryptography.HMACSHA256 cryptographer = new System.Security.Cryptography.HMACSHA256(keyBytes);

            byte[] bytes = cryptographer.ComputeHash(messageBytes);

            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
        public string HexString2B64String(string input)
        {
            return Convert.ToBase64String(HexStringToHex(input));
        }
        public byte[] HexStringToHex(string inputHex)
        {
            var resultantArray = new byte[inputHex.Length / 2];
            for (var i = 0; i < resultantArray.Length; i++)
            {
                resultantArray[i] = Convert.ToByte(inputHex.Substring(i * 2, 2), 16);
            }
            return resultantArray;
        }
    }
}
