using System;
using System.Collections.Generic;
using System.Text;
using IntraWebApi.Services.UserService.Model;

namespace IntraWebApi.Services.UserService
{
    public interface IUserService
    {
        void Create(UserModel user);
        string GetUser(string username, string password);
        void Update(UserModel user);
    }
}
