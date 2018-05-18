using IntraWebApi.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntraWebApi.Data.Models;

namespace IntraWebApi.Data.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly Context.Context _context;
        public ArticleRepository(Context.Context context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(int userId, string title, string content, DateTime creationDate, byte[] picture = null)
        {
            var userAccessRight = await _context.UserAccessRights.FirstOrDefaultAsync(u => u.UserId == userId);
            if (!userAccessRight.Write)
                return -1;

            var newArticle = new Article
            {
                Title = title,
                Content = content,
                Picture = picture,
                UserId = userId,
                CreationDate = creationDate
            };
            _context.Articles.Add(newArticle);
            await _context.SaveChangesAsync();
            return newArticle.Id;
        }

        public async Task<SystemResponse> DeleteAsync(int articleId, int userId)
        {
            var userAccessRight = await _context.UserAccessRights.FirstOrDefaultAsync(u => u.UserId == userId);
            if (!userAccessRight.Delete)
                return SystemResponse.AccessDenied;

            var article = _context.Articles.FirstOrDefault(a => a.Id == articleId);
            if (article == null)
                return SystemResponse.NotFound;

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return SystemResponse.Success;
        }

        public async Task<IEnumerable<Article>> GetAllArticlesAsync()
        {
            var articles = (from article in _context.Articles.OrderBy(a => a.CreationDate)
                            select article).AsQueryable();
            return  await articles.ToListAsync();
        }
		public async Task<SystemResponse> UpdateAsync(int articleId, int userId, string title = null, string content = null, byte[] picture = null)
        {
            var userAccessRight = await _context.UserAccessRights.FirstOrDefaultAsync(u => u.UserId == userId);
            if (!userAccessRight.Write)
                return SystemResponse.NotFound;

            var article = _context.Articles.FirstOrDefault(a => a.Id == articleId);
            if (article == null)
                return SystemResponse.NotFound;

            article.Title = string.IsNullOrEmpty(title) ? article.Title : title;
            article.Content = string.IsNullOrEmpty(content) ? article.Content : content;
            article.Picture = picture ?? article.Picture;
            await _context.SaveChangesAsync();
            return SystemResponse.Success;
        }

        public async Task<Article> GetArticleByIdAsync(int articleId)
        {
            return await _context.Articles.FirstOrDefaultAsync(a => a.Id == articleId);
        }
    }
}
