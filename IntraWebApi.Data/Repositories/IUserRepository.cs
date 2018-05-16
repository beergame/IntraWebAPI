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
        Task<SystemResponse> CreateUserAsync(UserRegister user);
        Task<User> GetUserAsync(string username, string password);
        Task<UserCredentials> GetUserCredentialsAsync(int userId);

        Task<SystemResponse> UpdateUserAsync(int userId, string firstname = null, string lastname = null,
            string password = null);
        Task<User> getUserByIdAsync(int userId);
    }
}
