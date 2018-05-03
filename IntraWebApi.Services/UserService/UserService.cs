using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IntraWebApi.Data.Context;
using IntraWebApi.Data.Models;
using IntraWebApi.Data.Repositories;
using IntraWebApi.Services.TokenProvider;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IntraWebApi.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenProvider.TokenProvider _tokenProvider;
        private const string SecretKey = "mysupersecret_secretkey!123";
        private const string UserRole = "user";
        private const string AdminRole = "admin";

        private static readonly SymmetricSecurityKey SigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        private readonly TokenProviderOptions _option = new TokenProviderOptions
        {
            Audience = "ExampleAudience",
            Issuer = "ExampleIssuer",
            SigningCredentials = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256),
        };

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _tokenProvider = new TokenProvider.TokenProvider(Options.Create(_option));
        }

        private string HashPassword(string password)
        {
            var provider = new SHA1CryptoServiceProvider();
            var encoding = new UnicodeEncoding();
            var encrypted = provider.ComputeHash(encoding.GetBytes(password));
            return Convert.ToBase64String(encrypted);
        }

        private async Task<User> GetUser(string username, string password)
        {
            var hashedPassword = HashPassword(password);
            return await _userRepository.GetUserAsync(username, hashedPassword);
        }

        private async Task<UserCredentials> GetUserCredentials(int userId)
        {
            return await _userRepository.GetUserCredentialsAsync(userId);
        }


        public async Task<SystemResponse> Create(string civility, string firstname, string lastname, string username, string password)
        {
            var hashedPassword = HashPassword(password);
            var user = new UserRegister
            {
                Civility = civility,
                FirstName = firstname,
                LastName = lastname,
                Username = username,
                PassWord = hashedPassword
            };
           return await _userRepository.CreateUserAsync(user);
        }

        public async Task<SystemResponse> UpdateAsync(string accessToken, string firstname = null, string lastname = null, string password = null)
        {
            if (string.IsNullOrEmpty(accessToken)) return SystemResponse.AccessDenied;

            var tokenDecoded = _tokenProvider.DecodeToken(accessToken);
            var userIdFromDictionary = tokenDecoded.Select(x => x.Key).First();
            int.TryParse(userIdFromDictionary, out var userId);

            if (string.IsNullOrEmpty(password))
                return await _userRepository.UpdateUserAsync(userId, firstname, lastname, password);

            var hashedPassword = HashPassword(password);
            return await _userRepository.UpdateUserAsync(userId, firstname, lastname, hashedPassword);
        }

        public async Task<Token> Authenticate(string username, string password)
        {
            var user = GetUser(username, password).Result;
            var userCredentials = GetUserCredentials(user.Id).Result;
            var role = UserRole;
            if (userCredentials.IsAdmin)
                role = AdminRole;

            return await _tokenProvider.GenerateTokenAsync(user.Id, userCredentials.Username, role);
        }
    }
}
