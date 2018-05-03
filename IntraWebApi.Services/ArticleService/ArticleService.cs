using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntraWebApi.Data.Context;
using IntraWebApi.Data.Models;
using IntraWebApi.Data.Repositories;
using IntraWebApi.Services.TokenProvider;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IntraWebApi.Services.ArticleService
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly TokenProvider.TokenProvider _tokenProvider;
        private const string SecretKey = "mysupersecret_secretkey!123";

        private static readonly SymmetricSecurityKey SigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        private readonly TokenProviderOptions _option = new TokenProviderOptions
        {
            Audience = "ExampleAudience",
            Issuer = "ExampleIssuer",
            SigningCredentials = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256),
        };

        public ArticleService(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
            _tokenProvider = new TokenProvider.TokenProvider(Options.Create(_option));
        }

        public async Task<int> CreateArticleAsync(string accessToken, string title, string content, DateTime creationDate, byte[] picture = null)
        {
            if (string.IsNullOrEmpty(accessToken))
                return -1;

            var userId = GetUserId(accessToken);
            var articleId = await _articleRepository.CreateAsync(userId, title, content, creationDate, picture);
            return articleId;
        }

        public async Task<SystemResponse> DeleteArticleAsync(int articleId, string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                return SystemResponse.AccessDenied;

            var userId = GetUserId(accessToken);
            var result = await _articleRepository.DeleteAsync(articleId, userId);
            return result;
        }

        public async Task<SystemResponse> UpdateArticleAsync(int articleId, string accessToken, string title = null, string content = null, byte[] picture = null)
        {
            if (string.IsNullOrEmpty(accessToken))
                return SystemResponse.AccessDenied;

            var userId = GetUserId(accessToken);
            var result = await _articleRepository.UpdateAsync(articleId, userId, title, content, picture);
            return result;
        }

        private int GetUserId(string accessToken)
        {
            var tokenDecoded = _tokenProvider.DecodeToken(accessToken);
            var userIdFromDictionary = tokenDecoded.Select(x => x.Key).First();
            int.TryParse(userIdFromDictionary, out var userId);
            return userId;
        }

        public async Task<IEnumerable<Article>> GetallArticlesAsync()
        {
            return await _articleRepository.GetAllArticlesAsync();
        }
    }
}
