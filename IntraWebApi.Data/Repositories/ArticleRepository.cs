using IntraWebApi.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<string> DeleteAsync(int articleId, int userId)
        {
            var userAccessRight = await _context.UserAccessRights.FirstOrDefaultAsync(u => u.UserId == userId);
            if (!userAccessRight.Delete)
                return "You have not access right for delete this article";

            var article = _context.Articles.FirstOrDefault(a => a.Id == articleId);
            if (article == null)
                return $"Article with {articleId} not found";

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return "Article deleted successfuly.";
        }

        public async Task<IEnumerable<Article>> GetAllArticlesAsync()
        {
            var articles = (from article in _context.Articles.OrderBy(a => a.CreationDate)
                            select article).AsQueryable();
            return  await articles.ToListAsync();
        }

        public async Task<string> UpdateAsync(int articleId, int userId, string title = null, string content = null, byte[] picture = null)
        {
            var userAccessRight = await _context.UserAccessRights.FirstOrDefaultAsync(u => u.UserId == userId);
            if (!userAccessRight.Write)
                return "User has no access right for this action";

            var article = _context.Articles.FirstOrDefault(a => a.Id == articleId);
            if (article == null)
                return $"Article with {articleId} not found";

            article.Title = string.IsNullOrEmpty(title) ? article.Title : title;
            article.Content = string.IsNullOrEmpty(content) ? article.Content : content;
            article.Picture = picture ?? article.Picture;
            await _context.SaveChangesAsync();
            return $"Article {articleId} is updated successfuly.";
        }
    }
}
