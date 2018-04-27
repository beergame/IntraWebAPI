using IntraWebApi.Data.Context;
using IntraWebApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace IntraWebApi.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _userContext;

        public UserRepository(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task CreateUserAsync(UserRegister user)
        {
            var newUser = new User
            {
                Civility = user.Civility,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            _userContext.Users.Add(newUser);
            await _userContext.SaveChangesAsync();
            var userId = newUser.Id;

            var userCred = new UserCredentials
            {
                UserId = userId,
                Username = user.Username,
                Password = user.PassWord,
                IsAdmin = false
            };
            _userContext.UserCredentials.Add(userCred);
            await _userContext.SaveChangesAsync();
        }

        public async Task<User> GetUserAsync(string username, string password)
        {
            var userCredentialsQuery = from u in _userContext.UserCredentials
                where u.Username == username /*&& u.Password == password*/
                select u;
            var userCredentials = await userCredentialsQuery.FirstAsync();
            if (userCredentials != null)
                return await _userContext.Users.FirstOrDefaultAsync(u => u.Id == userCredentials.UserId);
            return null;
        }
    }
}
