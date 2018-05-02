using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IntraWebApi.Services.TokenProvider
{
    public interface ITokenProvider
    {
        Task<Token> GenerateTokenAsync(int userId, string username, string role);
        Dictionary<string, string> DecodeToken(string accessToken);
    }
}
