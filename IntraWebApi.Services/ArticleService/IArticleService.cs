using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IntraWebApi.Data.Context;
using IntraWebApi.Data.Models;

namespace IntraWebApi.Services.ArticleService
{
    public interface IArticleService
    {
        Task<int> CreateArticleAsync(string accessToken, string title, string content, DateTime creationDate, byte[] picture = null);
        Task<SystemResponse> UpdateArticleAsync(int articleId, string accessToken, string title = null, string content = null, byte[] picture = null);
        Task<SystemResponse> DeleteArticleAsync(int articleId, string accessToken);
        Task<IEnumerable<Article>> GetallArticlesAsync();
    }
}
