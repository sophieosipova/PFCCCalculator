using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutorizationService.Database;
using AutorizationService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        public OAUTHController(UserManager<UserAccount> userManager,
           SignInManager<UserAccount> signInManager,
           RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
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

              if(!this.User.Identity.IsAuthenticated)          
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
        [Route("callback")]
        public async Task<ActionResult<string>> Authorize([FromQuery]string client_id = "app", [FromQuery]string redirect_uri = "https://localhost:44358/api/account", [FromQuery]string response_type = "code")
        {

            var acccount = appManager.GetApp(client_id);

            if (acccount != null)
            {
                var jwt = AutorizationCodeGenerator.GetAutorizationCode();
                acccount.AuthCode = jwt;
               // await userManager.UpdateAsync(acccount);
                await RedirectPermanent($"{redirect_uri}?code={jwt}").ExecuteResultAsync(new ActionContext());
            }
            return BadRequest();
        }
    }
}
 