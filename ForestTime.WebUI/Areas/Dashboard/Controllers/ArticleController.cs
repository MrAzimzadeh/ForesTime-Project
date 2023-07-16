using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ForestTime.WebUI.Data;
using ForestTime.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechBlog.WebUI.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize]
    public class ArticleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public ArticleController(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var articles = _context.Articles.Include(c => c.Category).Include(x => x.User).Include(y => y.ArticleTags).ThenInclude(z => z.Tag).ToList();
            return View(articles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var tagList = _context.Tags.ToList();
            ViewBag.Tags = new SelectList(tagList, "Id", "TagName");
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "CategoryName");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Article article, List<int> tagIds, int categoryId)
        {
            try
            {
                article.UserId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                article.SeoUrl = "dasdjaskd";
                article.CreatedDate = DateTime.Now;
                article.UpdatedDate = DateTime.Now;
                article.CategoryId = categoryId;
                await _context.Articles.AddAsync(article);
                await _context.SaveChangesAsync();
                for (int i = 0; i < tagIds.Count; i++)
                {
                    ArticleTag articleTag = new()
                    {
                        TagId = tagIds[i],
                        ArticleId = article.Id
                    };
                    await _context.ArticleTags.AddAsync(articleTag);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(article);

            }
        }
        public IActionResult Edit(int id)
        {
            var article = _context.Articles.Include(x => x.ArticleTags).FirstOrDefault(x => x.Id == id);
            if (article == null || id == null)
            {
                return NotFound();
            }

            var tags = _context.Tags.ToList();
            var categories = _context.Categories.ToList();

            ViewData["taglist"] = tags;
            ViewData["catlist"] = categories;

            return View(article);
        }

        [HttpPost]
        public IActionResult Edit(Article article, List<int> tagIds, int categoryId)
        {
            try
            {
                var exArticle = _context.Articles
                    .Include(a => a.ArticleTags)
                    .FirstOrDefault(a => a.Id == article.Id);

                if (exArticle == null)
                {
                    return NotFound();
                }

                exArticle.Title = article.Title;
                exArticle.Content = article.Content;
                exArticle.PhotoUrl = article.PhotoUrl;
                exArticle.UserId = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                exArticle.SeoUrl = "swd";
                exArticle.UpdatedDate = DateTime.Now;
                exArticle.CategoryId = categoryId;

                // Remove existing article tags
                _context.ArticleTags.RemoveRange(exArticle.ArticleTags);

                // Add new article tags
                foreach (int tagId in tagIds)
                {
                    ArticleTag articleTag = new ArticleTag
                    {
                        TagId = tagId,
                        ArticleId = exArticle.Id
                    };

                    _context.ArticleTags.Add(articleTag);
                }

                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(article);
            }
        }

    }
}
