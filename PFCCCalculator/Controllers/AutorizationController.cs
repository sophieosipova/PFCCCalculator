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
         //   if (!ModelState.IsValid)
           //     return BadRequest(ModelState.ToString());
            try
            {
                var logged= await autorizationService.Login(user);
                if (logged == null)
                    return BadRequest();
                return Ok(logged);
            }
            catch
            {
                return BadRequest();
            }
            
        }

        [HttpPost]
        [Route("refreshtokens")]
        public async Task<ActionResult<UsersToken>> RefreshTokens(UsersToken usersToken)
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

    }
}