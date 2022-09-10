using System;
using System.Collections.Generic;
using System.Text;

using System.IdentityModel.Tokens.Jwt;

namespace SharedAutorizationOptions
{
    public interface ITokenGenerator
    {
        //   JwtSecurityToken GenerateJwtToken(string id, string userName);

        string GenerateRefreshToken(string id, string userName);

        string GenerateAccessToken(string id, string userName);
        bool ValidateToken(string token);
        string GetUsernameFromToken(string token);
    }
}
