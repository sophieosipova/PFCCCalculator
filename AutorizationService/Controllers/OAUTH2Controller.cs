using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutorizationService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharedModels;

namespace AutorizationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OAUTH2Controller : ControllerBase
    {
        private readonly UserManager<AppAccount> userManager;
        private readonly SignInManager<AppAccount> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public OAUTH2Controller(
           UserManager<AppAccount> userManager,
           SignInManager<AppAccount> signInManager,
           RoleManager<IdentityRole> roleManager
       )
        {

            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
    /*        var p = userManager.FindByNameAsync("App");
          if ( p == null)
            {
                var app = new UserAccount { UserName = "App" };
                var result = userManager.CreateAsync(app, "qwe123456");

                userManager.AddClaimAsync(app, new Claim("userName", app.UserName));

              //  userManager.AddToRoleAsync(app, "app");
            }*/

        }


      /*  [HttpGet, AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<IdentityUser>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(userManager.Users);
        }

       [HttpPost, AllowAnonymous]
        public async Task<ActionResult<AppAuth>> AuthorizeCode([FromBody] App app)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(app.ClientId, app.ClinetSecret, false, false);

                if (result.Succeeded)
                {
                    var acccount = await userManager.FindByNameAsync(app.ClientId);

                    if (acccount != null)
                    {
                        var jwt = AutorizationCodeGenerator.GetAutorizationCode();
                        acccount.Token = jwt;
                        await userManager.UpdateAsync(acccount);
                        return new AppAuth()
                        {
                            AuthorizationCode = jwt,
                            ClientId = app.ClientId
                        
                        };

                    }

                }

            }
            return BadRequest(ModelState);
        }



        */

        [HttpGet, AllowAnonymous]
        public async Task<ActionResult<string>> AuthorizeRequest([FromQuery]string client_id="app", [FromQuery]string redirect_uri= "https://localhost:44358/api/account")
        {

             var acccount = await userManager.FindByNameAsync(client_id);

            if (acccount != null)
            {
                var jwt = AutorizationCodeGenerator.GetAutorizationCode();
           //     acccount.Token = jwt;
                await userManager.UpdateAsync(acccount);
                return RedirectPermanent(redirect_uri);
            }
            return BadRequest();
        }

        /*       [HttpGet, AllowAnonymous]
               public async Task<ActionResult<string>> Authorize([FromQuery]string client_id, [FromQuery]string redirect_uri)
               {

                   var acccount = await userManager.FindByNameAsync(client_id);

                   if (acccount != null)
                   {
                       var jwt = AutorizationCodeGenerator.GetAutorizationCode();
                       acccount.Token = jwt;
                       await userManager.UpdateAsync(acccount);
                       return RedirectPermanent(redirect_uri);
                   }
                   return BadRequest();
               }



               [HttpGet, AllowAnonymous]
               public async Task<ActionResult<AppAuth>> Authorize1([FromQuery]string client_id, [FromQuery]string redirect_uri)
               {

                   var acccount = await userManager.FindByNameAsync(client_id);

                   if (acccount != null)
                   {
                       var jwt = AutorizationCodeGenerator.GetAutorizationCode();
                       acccount.Token = jwt;
                       await userManager.UpdateAsync(acccount);
                       return new AppAuth()
                       {
                           AuthorizationCode = jwt,
                           ClientId = client_id

                       };

                   }
                   return BadRequest();
               }
               */

        /*   [HttpGet, AllowAnonymous]
           [Route("callback")]
           public async Task<ActionResult<AppAuth>> Oauth2Login([FromBody] AppAuth appAuth,[FromQuery]string code)
           {

               var acccount = await userManager.FindByNameAsync(appAuth.ClientId);

               if (acccount != null)
               {
                   var jwt = AutorizationCodeGenerator.GetAutorizationCode();
                   acccount.Token = jwt;
                   await userManager.UpdateAsync(acccount);
                   return new AppAuth()
                   {
                       AuthorizationCode = jwt,
                       ClientId = client_id

                   };

               }
               return BadRequest();
           }*/

    }
}