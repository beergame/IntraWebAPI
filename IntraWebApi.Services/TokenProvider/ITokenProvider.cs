using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IntraWebApi.Services.TokenProvider
{
    public interface ITokenProvider
    {
        Task<Token> GenerateToken(string username, string password);
    }
}
