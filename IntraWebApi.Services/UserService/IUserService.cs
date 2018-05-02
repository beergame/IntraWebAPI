using System.Threading.Tasks;
using IntraWebApi.Services.TokenProvider;

namespace IntraWebApi.Services.UserService
{
    public interface IUserService
    {
        Task<string> Create(string civility, string firstname, string lastname, string username, string password);
        Task<Token> Authenticate(string username, string password);
        Task<string> UpdateAsync(string accessToken, string firstname = null, string lastname = null, string password = null);
    }
}
