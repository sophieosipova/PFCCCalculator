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

    }
}
