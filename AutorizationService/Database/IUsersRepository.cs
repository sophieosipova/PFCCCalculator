using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutorizationService.Database
{

    public interface IUsersRepository
    {
        //    Task<List<Comment>> GetComments();
        Task<List<User>> GetUsers();
    }
}
