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

        public void DeleteNewsArticle(NewsArticle newsArticle)
        {
            iNewsArticleRepository.DeleteNewsArticle(newsArticle);
        }

        public NewsArticle GetNewsArticleById(int id)
        {
            return iNewsArticleRepository.GetNewsArticleById(id);
        }

        public List<NewsArticle> GetNewsArticles()
        {
            return iNewsArticleRepository.GetNewsArticles();
        }

        public void UpdateNewsArticle(NewsArticle newsArticle)
        {
            iNewsArticleRepository.UpdateNewsArticle(newsArticle);
        }
    }
}
