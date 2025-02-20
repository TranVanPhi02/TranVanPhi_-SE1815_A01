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

                listNewsArticles = db.NewsArticles
                             .Include(a => a.Category)    
                             .Include(a => a.CreatedBy)  
                             .Include(a => a.Tags)        
                             .ToList();

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

            
                var newsArticleToDelete = context.NewsArticles
                    .Include(a => a.Tags)   
                    .Include(a => a.Category) 
                    .SingleOrDefault(a => a.NewsArticleId == newsArticle.NewsArticleId);

              
                if (newsArticleToDelete != null)
                {
               
                    foreach (var tag in newsArticleToDelete.Tags.ToList())
                    {
                        context.Tags.Remove(tag);
                    }
                   
                    if (newsArticleToDelete.Category != null)
                    {
                        context.Categories.Remove(newsArticleToDelete.Category); 
                    }
       
                    context.NewsArticles.Remove(newsArticleToDelete);

                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("News article not found.");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error deleting news article: " + e.Message);
            }
        }


        public static NewsArticle GetNewsArticleById(int id)
        {
            using var db = new FunewsManagementContext();
            return db.NewsArticles.FirstOrDefault(a => a.NewsArticleId.Equals(id));
        }
    }
}
