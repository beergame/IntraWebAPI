using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IntraWebApi.Services.TokenProvider
{
    public class TokenProvider : ITokenProvider
    {
        private readonly TokenProviderOptions _options;
      
        public TokenProvider(IOptions<TokenProviderOptions> options)
        {
            _options = options.Value;
            ThrowIfInvalidOptions(_options);
        }

        public async Task<Token> GenerateTokenAsync(int userId, string username, string role)
        {
            var now = DateTime.UtcNow;

            // Specifically add the jti (nonce), iat (issued timestamp), and sub (subject/user) claims.
            var claims = new Claim[]
            {
                new Claim("sub", username),
                new Claim("jti", await _options.NonceGenerator()),
                new Claim("iat", ToUnixEpochDate(now).ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                    issuer: _options.Issuer,
                    audience: _options.Audience,
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(_options.Expiration),
                    signingCredentials: _options.SigningCredentials
                );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new Token
            {
                AccessToken = encodedJwt,
                ExpireIn = (int)_options.Expiration.TotalSeconds
            };

            return response;
        }

        public Dictionary<string, string> DecodeToken(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(accessToken);
            var userId = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            var role = token.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
            return new Dictionary<string, string>{{userId, role}};
        }

        private static void ThrowIfInvalidOptions(TokenProviderOptions options)
        {
            //if (string.IsNullOrEmpty(options.Path))
            //{
            //    throw new ArgumentNullException(nameof(TokenProviderOptions.Path));
            //}

            if (string.IsNullOrEmpty(options.Issuer))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Issuer));
            }

            if (string.IsNullOrEmpty(options.Audience))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Audience));
            }

            if (options.Expiration == TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(TokenProviderOptions.Expiration));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.SigningCredentials));
            }

            if (options.NonceGenerator == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.NonceGenerator));
            }
        }

        /// <summary>
        /// Get this datetime as a Unix epoch timestamp (seconds since Jan 1, 1970, midnight UTC).
        /// </summary>
        /// <param name="date">The date to convert.</param>
        /// <returns>Seconds since Unix epoch.</returns>
        public static long ToUnixEpochDate(DateTime date) => new DateTimeOffset(date).ToUniversalTime().ToUnixTimeSeconds();
    }

}
