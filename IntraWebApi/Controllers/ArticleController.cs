using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntraWebApi.Data.Models;
using IntraWebApi.Models;
using IntraWebApi.Services.ArticleService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IntraWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateArticle([FromBody]Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var accessToken = GetAccessTokenFormHeaders();
            var result = await _articleService.CreateArticleAsync(accessToken, article.Title, article.Content, DateTime.Now, article.Picture);

            return StatusCode(201, result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateArticle([FromBody] ArticleUpdate articleUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var accessToken = GetAccessTokenFormHeaders();
            var result = await _articleService.UpdateArticleAsync(articleUpdate.Id, accessToken, articleUpdate.Title,
                articleUpdate.Content, articleUpdate.Picture);
            switch (result)
            {
                case SystemResponse.AccessDenied:
                    return StatusCode(401);
                case SystemResponse.NotFound:
                    return NotFound(articleUpdate);
                default:
                    return Ok();
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteArticle([FromBody] int articleId)
        {
            var accessToken = GetAccessTokenFormHeaders();
            var result = await _articleService.DeleteArticleAsync(articleId, accessToken);
            switch (result)
            {
                case SystemResponse.AccessDenied:
                    return StatusCode(401);
                case SystemResponse.NotFound:
                    return NotFound(articleId);
                default:
                    return Ok();
            }
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllArticles([FromQuery] PagingParameter pagingParameter)
        {
            var articles = await _articleService.GetallArticlesAsync();
            if (articles == null)
                return NotFound();

            var count = articles.Count();
            var currentPage = pagingParameter.PageNumber;
            var pageSize = pagingParameter.PageSize;
            var totalCount = count;
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);
            var items = articles.ToList().Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            var previousPage = currentPage > 1 ? "Yes" : "No";
            var nextPage = currentPage < totalPages ? "Yes" : "No";
            var paginationMetadata = new
            {
                totalCount,
                pageSize,
                currentPage,
                totalPages,
                previousPage,
                nextPage
            };

            // Setting Header  
            Request.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));

            return Ok(items);
        }

        private string GetAccessTokenFormHeaders()
        {
            var headers = Request.Headers;
            string accessToken = null;
            var tokenIsFound = headers.TryGetValue("access_token", out var values);
            if (tokenIsFound)
                accessToken = values.FirstOrDefault();
            return accessToken;
        }
    }
}