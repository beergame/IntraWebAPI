using System;
using System.Collections.Generic;
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
        private static readonly string _secretKey = "mysupersecret_secretkey!123";

        private static readonly SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey));
        private readonly TokenProviderOptions _option = new TokenProviderOptions
        {
            Audience = "ExampleAudience",
            Issuer = "ExampleIssuer",
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
        };

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _tokenProvider = new TokenProvider.TokenProvider(Options.Create(_option));
        }

        private string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        private async Task<User> GetUser(string username, string password)
        {
            var hashedPassword = HashPassword(password);
            return await _userRepository.GetUserAsync(username, hashedPassword);
        }


        public async Task Create(string civility, string firstname, string lastname, string username, string password)
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
            await _userRepository.CreateUserAsync(user);
        }

        public Task Update(string username, string firstname = null, string lastname = null, string password = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Token> Authenticate(string username, string password)
        {
            var user = GetUser(username, password).Result;
            if (user != null)
                return await _tokenProvider.GenerateToken(username, password);
            return null;
        }
    }
}
