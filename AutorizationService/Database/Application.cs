using AutorizationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutorizationService.Database
{
    public class ApplicationRepository
    {
        private static List<AppAccount> TestUsers = new List<AppAccount>();

        public ApplicationRepository()
        {
            if(!TestUsers.Any())
                TestUsers.Add(new AppAccount { AppId = "app", AppSecret = "secret" });
        }
        public AppAccount GetApp(string appId)
        {
            try
            {
                return TestUsers.First(user => user.AppId.Equals(appId));
            }
            catch
            {
                return null;
            }
        }

        public void SetUser (string appId, string userName)
        {
            try
            {
          //      if (!TestUsers.First(user => user.AppId.Equals(appId)).AutorizedUsers.)
        //        {
                    string s;
                    if (!TestUsers.First(user => user.AppId.Equals(appId)).AutorizedUsers.TryGetValue(userName, out s))
                        TestUsers.First(user => user.AppId.Equals(appId)).AutorizedUsers.Add(new string(userName), "default");
               // }
            }
            catch
            {
                return;
            }
        }
    }

}
