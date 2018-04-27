using System.Threading.Tasks;
using IntraWebApi.Services.TokenProvider;

namespace IntraWebApi.Services.UserService
{
    public interface IUserService
    {
        Task Create(string civility, string firstname, string lastname, string username, string password);
        Task<Token> Authenticate(string username, string password);
        Task Update(string username, string firstname = null, string lastname = null, string password = null);
    }
}
