using BusinessObjects;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        private readonly NewsArticleDAO _newsArticleDAO;

        public NewsArticleRepository(NewsArticleDAO newsArticleDAO)
        {
            _newsArticleDAO = newsArticleDAO;
        }
        public void AddNewsArticle(NewsArticle newsArticle)=>NewsArticleDAO.AddNewsArticle(newsArticle);

        public void DeleteNewsArticle(string id)=>NewsArticleDAO.DeleteNewsArticle(id);

        public NewsArticle GetNewsArticleById(int id)=> NewsArticleDAO.GetNewsArticleById(id);  

        public List<NewsArticle> GetNewsArticles()=> NewsArticleDAO.GetNewsArticles();

        public async Task<List<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _newsArticleDAO.GetNewsByDateRangeAsync(startDate, endDate);
        }

        public void UpdateNewsArticle(NewsArticle newsArticle)=> NewsArticleDAO.UpdateNewsArticle(newsArticle);

     
    }
}
