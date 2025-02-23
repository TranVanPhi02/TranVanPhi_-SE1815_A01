using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository iNewsArticleRepository;
        public void AddNewsArticle(NewsArticle newsArticle)
        {
            iNewsArticleRepository.AddNewsArticle(newsArticle);
        }

        public void DeleteNewsArticle(string id)
        {
            iNewsArticleRepository.DeleteNewsArticle(id);
        }

        public NewsArticle GetNewsArticleById(int id)
        {
            return iNewsArticleRepository.GetNewsArticleById(id);
        }

        public List<NewsArticle> GetNewsArticles()
        {
            return iNewsArticleRepository.GetNewsArticles();
        }
        public async Task<List<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await iNewsArticleRepository.GetNewsByDateRangeAsync(startDate, endDate);
        }

        public void UpdateNewsArticle(NewsArticle newsArticle)
        {
            iNewsArticleRepository.UpdateNewsArticle(newsArticle);
        }
    }
}
