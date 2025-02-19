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
        public void AddNewsArticle(NewsArticle newsArticle)=>NewsArticleDAO.AddNewsArticle(newsArticle);

        public void DeleteNewsArticle(NewsArticle newsArticle)=>NewsArticleDAO.DeleteNewsArticle(newsArticle);

        public NewsArticle GetNewsArticleById(int id)=> NewsArticleDAO.GetNewsArticleById(id);  

        public List<NewsArticle> GetNewsArticles()=> NewsArticleDAO.GetNewsArticles();

        public void UpdateNewsArticle(NewsArticle newsArticle)=> NewsArticleDAO.UpdateNewsArticle(newsArticle);
    }
}
