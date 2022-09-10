using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels
{
    public class App
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public class AppAuth
    {
        public string AuthorizationCode { get; set; }
        public string ClientId { get; set; }
    }

    public class AppsToken
    {
        public string Token { get; set; }
        public string ClientId { get; set; }
    }
}
