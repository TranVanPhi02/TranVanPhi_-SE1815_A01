using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Services;

using static TranVanPhiMVC.Extensions.Mail;

namespace TranVanPhiMVC.Controllers
{
    public class NewsArticlesController : Controller
    {
        private readonly FunewsManagementContext _context;
        private readonly INewsArticleService _newsArticleService;
        private readonly IEmailSender _emailSender;
        public NewsArticlesController(IEmailSender emailSender, FunewsManagementContext context, INewsArticleService newsArticleService)
        {
            _emailSender = emailSender;
            _context = context;
            _newsArticleService = newsArticleService ?? throw new ArgumentNullException(nameof(newsArticleService)); 
        }


        // GET: NewsArticles
        public async Task<IActionResult> Index()
        {
            var funewsManagementContext = _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.NewsTags) 
                    .ThenInclude(nt => nt.Tag); 

            return View(await funewsManagementContext.ToListAsync());
        }


        // GET: NewsArticles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .FirstOrDefaultAsync(m => m.NewsArticleId == id);
            if (newsArticle == null)
            {
                return NotFound();
            }

            return View(newsArticle);
        }

        // GET: NewsArticles/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryDesciption");
            ViewData["CreatedById"] = new SelectList(_context.SystemAccounts, "AccountId", "AccountId");
            return View();
        }

        // POST: NewsArticles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NewsArticleId,NewsTitle,Headline,CreatedDate,NewsContent,NewsSource,CategoryId,NewsStatus,CreatedById,UpdatedById,ModifiedDate")] NewsArticle newsArticle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(newsArticle);
                await _context.SaveChangesAsync();

                // Gửi email thông báo cho Lecturer sau khi tạo bài viết thành công
                string emailSubject = $"New News Article Created: {newsArticle.NewsTitle}";
                string emailBody = $"A new news article titled '{newsArticle.NewsTitle}' has been created. Please review it.";
                var lecturers = _context.SystemAccounts
          .Where(a => a.AccountRole.HasValue && a.AccountRole.Value == 2) 
          .ToList();


                foreach (var lecturer in lecturers)
                {
                    await _emailSender.SendEmailAsync(lecturer.AccountEmail, emailSubject, emailBody);
                }


                TempData["SuccessMessage"] = "News article created successfully and email sent to the lecturer.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryDesciption", newsArticle.CategoryId);
            ViewData["CreatedById"] = new SelectList(_context.SystemAccounts, "AccountId", "AccountId", newsArticle.CreatedById);
            return View(newsArticle);
        }

        // GET: NewsArticles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = await _context.NewsArticles.FindAsync(id);
            if (newsArticle == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryDesciption", newsArticle.CategoryId);
            ViewData["CreatedById"] = new SelectList(_context.SystemAccounts, "AccountId", "AccountId", newsArticle.CreatedById);
            return View(newsArticle);
        }

        // POST: NewsArticles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("NewsArticleId,NewsTitle,Headline,CreatedDate,NewsContent,NewsSource,CategoryId,NewsStatus,CreatedById,UpdatedById,ModifiedDate")] NewsArticle newsArticle)
        {
            if (id != newsArticle.NewsArticleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newsArticle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsArticleExists(newsArticle.NewsArticleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryDesciption", newsArticle.CategoryId);
            ViewData["CreatedById"] = new SelectList(_context.SystemAccounts, "AccountId", "AccountId", newsArticle.CreatedById);
            return View(newsArticle);
        }

        // GET: NewsArticles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .FirstOrDefaultAsync(m => m.NewsArticleId == id);
            if (newsArticle == null)
            {
                return NotFound();
            }

            return View(newsArticle);
        }

        // POST: NewsArticles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Invalid news article ID.";
                return RedirectToAction(nameof(Index));
            }

            var newsArticle = await _context.NewsArticles
                .Include(na => na.NewsTags)
                .ThenInclude(nt => nt.Tag) // Bao gồm Tag để tránh lỗi khi xóa
                .FirstOrDefaultAsync(na => na.NewsArticleId == id);

            if (newsArticle == null)
            {
                TempData["ErrorMessage"] = $"The news article with ID {id} does not exist.";
                return RedirectToAction(nameof(Index));
            }

            // Xóa tất cả các NewsTag liên quan trước khi xóa NewsArticle
            if (newsArticle.NewsTags != null && newsArticle.NewsTags.Any())
            {
                _context.NewsTags.RemoveRange(newsArticle.NewsTags);
            }

            // Xóa NewsArticle
            _context.NewsArticles.Remove(newsArticle);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "News article deleted successfully.";
            return RedirectToAction(nameof(Index));
        }



 

        private bool NewsArticleExists(string id)
        {
            return _context.NewsArticles.Any(e => e.NewsArticleId == id);
        }
    }
}
