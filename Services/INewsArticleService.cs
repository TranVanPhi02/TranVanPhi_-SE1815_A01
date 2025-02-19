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
        void DeleteNewsArticle(NewsArticle newsArticle);
        NewsArticle GetNewsArticleById(int id);
    }
}
