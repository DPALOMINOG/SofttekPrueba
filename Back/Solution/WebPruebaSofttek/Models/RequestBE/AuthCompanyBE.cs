using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WebPruebaSofttek.Models.RequestBE
{
    [DataContract]
    public class AuthCompanyBE
    {
        [Required]
        public string usuario { get; set; } = string.Empty;
        [Required]
        public string clave { get; set; } = string.Empty;
        public String MaskCredencial()
        {
            var mask = String.Empty;
            mask = String.Format("Usuario: {0} Clave: {1}", usuario.Substring(0, 3).PadRight(10, '#'), clave.Substring(0, 3).PadRight(10, '#'));
            return mask;
        }
    }
}
