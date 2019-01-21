using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharedModels;

namespace PFCCCalculatorService.Services
{
    public interface  IAutorizationService
    {
        Task<UsersToken> Login(User user);
        Task<UsersToken> RefreshTokens(UsersToken token);

        Task<bool> ValidateToken(string accessToken);

        Task<ActionResult<string>> AuthorizeRequest(string client_id = "app",string redirect_uri = "https://localhost:44358/api/account", string response_type = "code");
       Task<ActionResult<UsersToken>> GetToken(string code, string client_secret = "secret", string client_id = "app", string redirect_uri = "https://localhost:44358/api/account");
     //  Task<ActionResult<string>> AuthorizeRequest(string client_id = "app", string redirect_uri = "https://localhost:44358/api/account");
    }
}
