using System.Threading.Tasks;
using IntraWebApi.Data.Models;
using IntraWebApi.Services.TokenProvider;

namespace IntraWebApi.Services.UserService
{
    public interface IUserService
    {
        Task<SystemResponse> Create(string civility, string firstname, string lastname, string username, string password);
        Task<Token> Authenticate(string username, string password);
        Task<SystemResponse> UpdateAsync(string accessToken, string firstname = null, string lastname = null, string password = null);
    }
}
