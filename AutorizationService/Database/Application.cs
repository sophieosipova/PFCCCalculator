﻿using AutorizationService.Models;
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
         //   TestUsers = new List<AppAccount>();
            TestUsers.Add(new AppAccount { AppId = "app", AppSecret = "secret" });
            //  TestUsers.Add(new AppAccount> () { Username = "Test2", Password = "Pass2" });
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
                TestUsers.First(user => user.AppId.Equals(appId)).AutorizedUsers.Add(new string (userName));
            }
            catch
            {
                return;
            }
        }
    }

}
