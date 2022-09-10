using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedAutorizationOptions;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsService.Controllers
{

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("api/[controller]")]
        [ApiController]
        public class AutorizationController : ControllerBase

        {

            private readonly ITokenGenerator tokenGenerator;
            private readonly App gateWay = new App() { ClientId = "Gateway", ClientSecret = "Sercret" };

            public AutorizationController(ITokenGenerator tokenGenerator)
            {
                this.tokenGenerator = tokenGenerator;
            }



            [HttpPost, AllowAnonymous]
            public ActionResult<AppsToken> Login([FromBody] App client)
            {
                if (ModelState.IsValid)
                {
                    if (gateWay.ClientId == client.ClientId &&
                        gateWay.ClientSecret == client.ClientSecret)

                        return new AppsToken()
                        {
                            Token = tokenGenerator.GenerateAccessToken(client.ClientId, client.ClientSecret),
                            ClientId = client.ClientId
                        };
                }
                return BadRequest(ModelState);
            }


        [HttpGet]
        public ActionResult<string> Hello()
        {
            return Ok("Hello");
        }


    }

}
    
