using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutorizationService.Models
{
    public class UserAccount : IdentityUser
    {
        public string Token { get; set; }

    }

    public class AutorizedUsers 
    {
        public string UserName { get; set; }
        public string AuthCode { get; set; }
    }

    public class AppAccount 
    {
        public string AppId;
        public string AppSecret;
        //  public string AuthCode;
        public Dictionary<string, string> AutorizedUsers = new Dictionary<string, string> ();
    }
}
