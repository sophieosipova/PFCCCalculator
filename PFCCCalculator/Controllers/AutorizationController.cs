using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFCCCalculatorService.Services;
using SharedModels;

namespace PFCCCalculatorService.Controllers
{
  //  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizationController : ControllerBase
    {
        private readonly IAutorizationService autorizationService;
        public AutorizationController(IAutorizationService autorizationService)
        {
            this.autorizationService = autorizationService;
        }

        [HttpPost/*, AllowAnonymous*/]
        public async Task<ActionResult<UsersToken>> Login([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ToString());
            try
            {
                var logged= await autorizationService.Login(user);
                if (logged == null)
                    return BadRequest();
                return logged;
            }
            catch
            {
                return BadRequest();
            }
            
        }

        [HttpPost]
        [Route("refreshtokens")]
        public  ActionResult<UsersToken> RefreshTokens(UsersToken usersToken)
        {
          //  if (!ModelState.IsValid)
           //     return BadRequest(ModelState.ToString());

            try
            {
                var token = autorizationService.RefreshTokens(usersToken);
                if (token != null)
                    return Ok(token);
            }
            catch
            {
                return BadRequest();
            }
                 
            return BadRequest();
        }


        [HttpGet]
        [Route("validate")]
        public async Task<ActionResult<bool>> Validate()
        {
             if (!ModelState.IsValid)
                return BadRequest(ModelState.ToString());
            try
            {
                string header = Request.Headers["Authorization"];

                bool flag = await autorizationService.ValidateToken(header);
                if (flag)
                    return Ok(flag);
            }
            catch
            {
                return BadRequest();
            }

            return Unauthorized();
        }


        [HttpGet]
        [Route("oauth")]
        public async Task<ActionResult<string>> AuthorizeRequest([FromQuery]string client_id = "app", [FromQuery]string redirect_uri = "https://localhost:44358/api/account", [FromQuery]string response_type = "code")
        {

            try
            {
                var  code = await autorizationService.AuthorizeRequest(client_id, redirect_uri, response_type);
                if (code != null)
                    return RedirectPermanent(code.Value);
            }
            catch
            {
                return BadRequest();
            }

            return BadRequest();
        }


        [HttpGet]
        [Route("token")]
        public async Task<ActionResult<UsersToken>> GetToken([FromQuery]string code, [FromQuery]string client_secret = "secret", [FromQuery]string client_id = "app", [FromQuery]string redirect_uri = "https://localhost:44358/api/account")
        {
            try
            {
                var token = await autorizationService.GetToken(code,client_secret, client_id, redirect_uri);
                if (token != null)
                    return Ok(token);
            }
            catch
            {
                return BadRequest();
            }

            return BadRequest();
        }
    }

}
