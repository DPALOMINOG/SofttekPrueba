using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helper
{
    public class AppSettings
    {
        public string Secret { get; set; }
    }
    public class Logging
    {
        public string LogEmailPath { get; set; }
        public string LogSTKPath { get; set; }
    }
    public class Security
    {
        public string PrivateKeyJWT { get; set; }
        public string IV { get; set; }
    }
    public class Url
    {
        public string STKDeploy { get; set; }
        public string STKWeb { get; set; }
        public string STKCore { get; set; }
    }
}
