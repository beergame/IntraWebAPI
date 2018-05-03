using IntraWebApi.Data.Context;
using IntraWebApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Edm.Csdl;

namespace IntraWebApi.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Context.Context _context;

        public UserRepository(Context.Context context)
        {
            _context = context;
        }

        public async Task<SystemResponse> CreateUserAsync(UserRegister user)
        {
            var newUser = new User
            {
                Civility = user.Civility,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            var userId = newUser.Id;
           
            var userCred = new UserCredentials
            {
                UserId = userId,
                Username = user.Username,
                Password = user.PassWord,
                IsAdmin = false
            };
            _context.UserCredentials.Add(userCred);
            var canDelete = userCred.IsAdmin;
            var userDefaultAccessRight = new UserAccessRight
            {
                UserId = userId,
                Read = true,
                Write = true,
                Delete = canDelete
            };
            _context.UserAccessRights.Add(userDefaultAccessRight);

            await _context.SaveChangesAsync();
            return SystemResponse.Success;
        }

        public async Task<User> GetUserAsync(string username, string password)
        {
            var userCredentialsQuery = from u in _context.UserCredentials
                where u.Username == username && u.Password == password
                select u;
            var userCredentials = await userCredentialsQuery.FirstAsync();
            if (userCredentials != null)
                return await _context.Users.FirstOrDefaultAsync(u => u.Id == userCredentials.UserId);
            return null;
        }

        public async Task<UserCredentials> GetUserCredentialsAsync(int userId)
        {
            return await _context.UserCredentials.FirstOrDefaultAsync(uc => uc.UserId == userId);
        }

        public async Task<SystemResponse> UpdateUserAsync(int userId, string firstname = null, string lastname = null, string password = null)
        {
            var userToUpdate = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (userToUpdate == null) return SystemResponse.NotFound;

            userToUpdate.FirstName = string.IsNullOrEmpty(firstname) ? userToUpdate.FirstName : firstname;
            userToUpdate.LastName = string.IsNullOrEmpty(lastname) ? userToUpdate.LastName : lastname;
            if (!string.IsNullOrEmpty(password))
            {
                var userToUpdateCredentials =
                    await _context.UserCredentials.FirstOrDefaultAsync(uc => uc.UserId == userId);
                userToUpdateCredentials.Password = password;
            }
             
            await _context.SaveChangesAsync();
            return SystemResponse.Success;
        }
    }
}
