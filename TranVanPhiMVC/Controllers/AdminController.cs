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
            _newsArticleService = newsArticleService;
        }

        public IActionResult Report()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReport(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                ViewBag.ErrorMessage = "Start date must be earlier than End date!";
                return View("Report");
            }

            var reportData = await _newsArticleService.GetNewsByDateRangeAsync(startDate, endDate);
            return View("Report", reportData);
        }
    }
}
