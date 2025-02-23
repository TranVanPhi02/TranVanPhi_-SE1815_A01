using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface INewsArticleService
    {
        List<NewsArticle> GetNewsArticles();
        void AddNewsArticle(NewsArticle newsArticle);
        void UpdateNewsArticle(NewsArticle newsArticle);
        void DeleteNewsArticle(string id);
        NewsArticle GetNewsArticleById(int id);
        Task<List<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
