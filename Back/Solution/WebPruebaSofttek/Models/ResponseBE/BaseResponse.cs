using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebPruebaSofttek.Models.ResponseBE
{
    [DataContract]
    public class Auth : BaseResponse
    {
        [JsonProperty(Order = 100)]
        [DataMember]
        public String expira_en { get; set; } = String.Empty;

        [JsonProperty(Order = 110)]
        [DataMember]
        public String token { get; set; } = String.Empty;

        [JsonProperty(Order = 120)]
        [DataMember]
        public String refresh_token { get; set; } = String.Empty;
    }
    [DataContract]
    public class BaseResponse
    {
        [JsonProperty(Order = 1)]
        [DataMember]
        public String codigoRespuesta { get; set; } = "99";
        [JsonProperty(Order = 2)]
        [DataMember]
        public String mensaje { get; set; } = string.Empty;//"ERROR DE SISTEMA";  
    }
}
