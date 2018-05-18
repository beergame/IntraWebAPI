using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using IntraWebApi.Data.Context;
using IntraWebApi.Data.Models;

namespace IntraWebApi.Data.Repositories
{
    public interface IArticleRepository
    {
        Task<int> CreateAsync(int userId, string title, string content, DateTime creationDate, byte[] picture = null);
        Task<SystemResponse> UpdateAsync(int articleId, int userId, string title = null, string content = null, byte[] picture = null);
        Task<SystemResponse> DeleteAsync(int articleId, int userId);
        Task<IEnumerable<Article>> GetAllArticlesAsync();
        Task<Article> GetArticleByIdAsync(int articleId);
    }
}
