using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using IntraWebApi.Data.Context;

namespace IntraWebApi.Data.Repositories
{
    public interface IArticleRepository
    {
        Task<int> CreateAsync(int userId, string title, string content, DateTime creationDate, byte[] picture = null);
        Task<string> UpdateAsync(int articleId, int userId, string title = null, string content = null, byte[] picture = null);
        Task<string> DeleteAsync(int articleId, int userId);
        Task<IEnumerable<Article>> GetAllArticlesAsync();
    }
}
