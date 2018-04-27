using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IntraWebApi.Data.Context;
using IntraWebApi.Data.Models;

namespace IntraWebApi.Data.Repositories
{
    public interface IUserRepository
    {
        Task CreateUserAsync(UserRegister user);
        Task<User> GetUserAsync(string username, string password);
    }
}
