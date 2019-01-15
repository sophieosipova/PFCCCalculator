using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutorizationService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharedAutorizationOptions;
using SharedModels;

namespace AutorizationService.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly ITokenGenerator tokenGenerator;

        private readonly UserManager<UserAccount> userManager;
        private readonly SignInManager<UserAccount> signInManager;
     //   private readonly RoleManager<IdentityRole> _roleManager;


        public AccountController(

            ITokenGenerator tokenGenerator,
            UserManager<UserAccount> userManager,
            SignInManager<UserAccount> signInManager//,
        //    RoleManager<IdentityRole> roleManager
        )
        {

            this.tokenGenerator = tokenGenerator;
            this.userManager = userManager;
            this.signInManager = signInManager;
            // this._roleManager = roleManager;

            if (!userManager.Users.Any())
            {
                var user = new UserAccount { UserName = "Sophie" };
                var result = userManager.CreateAsync(user, "qwe123456");

                userManager.AddClaimAsync(user, new Claim("userName", user.UserName));
            }

         }


        [HttpGet, AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<IdentityUser>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsers()
        {
           return Ok (userManager.Users);
        }

        [HttpPost, AllowAnonymous]
        public async Task<ActionResult<UsersToken>> Login([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(user.Username, user.Password, false, false);

                if (result.Succeeded)
                {
                    var acccount = await userManager.FindByNameAsync(user.Username);

                    if (acccount != null)
                    {
                        var jwt = tokenGenerator.GenerateRefreshToken(acccount.Id, acccount.UserName);
                        acccount.Token = jwt;
                        await userManager.UpdateAsync(acccount);
                        return new UsersToken()
                        {
                            RefreshToken = jwt,
                            AccessToken = tokenGenerator.GenerateAccessToken(acccount.Id, acccount.UserName),
                            UserName = user.Username
                        };

                    }

                }

            }
            return BadRequest(ModelState);
        }


       [HttpPost, Route("refreshtokens")]
        public async Task<ActionResult<UsersToken>> RefreshTokens(UsersToken usersToken)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                
                string token = Request.Headers["Authorization"].ToString().Remove(0,7);
                var user = await userManager.FindByNameAsync(User.Identity.Name/*usersToken.UserName*/);

                if (user != null && user.Token == token /*usersToken.RefreshToken*/)
                {
                    if (tokenGenerator.ValidateToken(usersToken.RefreshToken))
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
