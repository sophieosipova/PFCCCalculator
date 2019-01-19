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

        public OAUTHController(     ITokenGenerator tokenGenerator, UserManager<UserAccount> userManager,
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
        public async Task<ActionResult<string>> AuthorizeRequest([FromQuery]string client_id = "app", [FromQuery]string redirect_uri = "https://localhost:44358/api/account", [FromQuery]string response_type= "code")
        {

            var acccount = appManager.GetApp(client_id);//await userManager.FindByNameAsync(client_id);

            if (acccount != null)
            {
               // var jwt = AutorizationCodeGenerator.GetAutorizationCode();
                //     acccount.Token = jwt;
                //await userManager.UpdateAsync(acccount);

              if(!this.User.Identity.IsAuthenticated || !acccount.AutorizedUsers.Contains(this.User.Identity.Name))          
                   return RedirectPermanent("https://localhost:44358/Auth.html?client_id=app");

                var jwt = AutorizationCodeGenerator.GetAutorizationCode();
                acccount.AuthCode = jwt;
                // await userManager.UpdateAsync(acccount);
                this.Response.Headers.Add("Content-Type", "application/json");
                return  RedirectPermanent($"{redirect_uri}?code={jwt}");//$"{redirect_uri}?code={jwt}");
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
                if (acccount.AppSecret == client_secret && acccount.AuthCode == code)
                {
                    var user = await userManager.FindByNameAsync(User.Identity.Name);
                    if (user != null)
                    {
                        var jwt = tokenGenerator.GenerateRefreshToken(user.Id, user.UserName);
                        user.Token = jwt;
                        await userManager.UpdateAsync(user);
                        return new UsersToken()
                        {
                            AccessToken = tokenGenerator.GenerateAccessToken(user.Id, user.UserName),
                            RefreshToken = jwt,
                            UserName = user.UserName
                        };
                    }
                }
            }
            return BadRequest();
        }
    }
}
 