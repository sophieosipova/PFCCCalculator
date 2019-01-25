using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutorizationService.Database;
using AutorizationService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharedAutorizationOptions;
using SharedModels;

namespace AutorizationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OAUTHController : ControllerBase
    {

        private readonly ApplicationRepository appManager;
        // private readonly SignInManager<AppAccount> signInManager;
        //  private readonly RoleManager<IdentityRole> roleManager;

        private readonly UserManager<UserAccount> userManager;
        private readonly SignInManager<UserAccount> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ITokenGenerator tokenGenerator;

        public OAUTHController( ITokenGenerator tokenGenerator, UserManager<UserAccount> userManager,
           SignInManager<UserAccount> signInManager,
           RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.tokenGenerator = tokenGenerator;
            appManager = new ApplicationRepository(); 
        }


        [HttpGet]
        public  ActionResult<string> AuthorizeRequest([FromQuery]string client_id = "app", [FromQuery]string redirect_uri = "https://localhost:44358/api/account", [FromQuery]string response_type= "code")
        {

            var acccount = appManager.GetApp(client_id);
            string s;
            if (acccount != null)
            {
              if(!this.User.Identity.IsAuthenticated || !acccount.AutorizedUsers.TryGetValue(User.Identity.Name, out s))          
                   return Redirect($"https://localhost:44358/Auth.html?client_id={client_id}&redirect_uri={redirect_uri}&response_type={response_type}");

                var jwt = AutorizationCodeGenerator.GetAutorizationCode();
                acccount.AutorizedUsers[User.Identity.Name] = jwt;
                return Redirect($"{redirect_uri}?code={jwt}");
            }
            return BadRequest();
        }


        [HttpGet]
        [Route("token")]
        public async Task<ActionResult<UsersToken>> GetToken([FromQuery]string code, [FromQuery]string client_secret = "secret", [FromQuery]string client_id = "app", [FromQuery]string redirect_uri = "https://localhost:44358/api/account")
        {

            var acccount = appManager.GetApp(client_id);

            if (acccount != null )
            {
                if (acccount.AppSecret == client_secret && acccount.AutorizedUsers.Values.Contains(code))
                {
                    var user = await userManager.FindByNameAsync(acccount.AutorizedUsers.Where(x => x.Value == code).FirstOrDefault().Key);
                    if (user != null)
                    {
                        var jwt = tokenGenerator.GenerateRefreshToken(user.Id, user.UserName);
                        user.Token = jwt;
                        await userManager.UpdateAsync(user);
                        
                        UsersToken usersToken =  new UsersToken()
                        {
                            AccessToken = tokenGenerator.GenerateAccessToken(user.Id, user.UserName),
                            RefreshToken = jwt,
                            UserName = user.UserName,
                            UserId = user.Id
                        };
                        acccount.AutorizedUsers.Remove(user.UserName);
                        return usersToken;
                    }
                }
            }
            return BadRequest();
        }
    }
}
 