using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TranVanPhiMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly INewsArticleService _newsArticleService;

        public AdminController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService ?? throw new ArgumentNullException(nameof(newsArticleService));
        }

        public IActionResult Report()
        {
            return View(new List<NewsArticle>()); 
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReport(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                ViewBag.ErrorMessage = "Please select both start and end dates!";
                return View("Report"); 
            }

            if (startDate > endDate)
            {
                ViewBag.ErrorMessage = "Start date must be earlier than End date!";
                return View("Report");
            }

            try
            {
                var reportData = await _newsArticleService.GetNewsByDateRangeAsync(startDate.Value, endDate.Value);

                if (reportData == null || !reportData.Any())
                {
                    ViewBag.ErrorMessage = "No news articles found for the selected date range.";
                }

                return View("Report", reportData); 
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View("Report");
            }
        }


    }
}
