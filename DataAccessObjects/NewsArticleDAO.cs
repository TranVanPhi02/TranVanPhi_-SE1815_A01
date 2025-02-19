using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class NewsArticleDAO
    {
        public static List<NewsArticle> GetNewsArticles()
        {
            var listNewsArticles = new List<NewsArticle>();
            try
            {

                using var db = new FunewsManagementContext();

                listNewsArticles = db.NewsArticles.Include(a => a.Category).ToList();

            }
            catch (Exception e) { }
            return listNewsArticles;
        }

        public static void AddNewsArticle(NewsArticle newsArticle)
        {
            try
            {
                using var context = new FunewsManagementContext();
                context.NewsArticles.Add(newsArticle);
                context.SaveChanges(); // Update Database
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public static void UpdateNewsArticle(NewsArticle newsArticle)
        {
            try
            {
                using var context = new FunewsManagementContext();
                context.Entry<NewsArticle>(newsArticle).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public static void DeleteNewsArticle(NewsArticle newsArticle)
        {
            try
            {
                using var context = new FunewsManagementContext();
                var newsArticleList = context.NewsArticles.SingleOrDefault(a => a.NewsArticleId == newsArticle.NewsArticleId);
                context.NewsArticles.Remove(newsArticleList);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static NewsArticle GetNewsArticleById(int id)
        {
            using var db = new FunewsManagementContext();
            return db.NewsArticles.FirstOrDefault(a => a.NewsArticleId.Equals(id));
        }
    }
}
