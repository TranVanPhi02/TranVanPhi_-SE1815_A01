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
        private readonly FunewsManagementContext _context;

        public NewsArticleDAO(FunewsManagementContext context)
        {
            _context = context;
        }
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

        public static void DeleteNewsArticle(string id)
        {
            try
            {
                using var context = new FunewsManagementContext();

                var newsArticleToDelete = context.NewsArticles
                    .Include(a => a.NewsTags) // Bao gồm NewsTags
                    .ThenInclude(nt => nt.Tag) // Bao gồm Tag để tránh lỗi
                    .FirstOrDefault(a => a.NewsArticleId == id);

                if (newsArticleToDelete == null)
                {
                    throw new Exception($"News article with ID {id} not found.");
                }

                // Xóa tất cả các NewsTag liên quan trước
                if (newsArticleToDelete.NewsTags != null && newsArticleToDelete.NewsTags.Any())
                {
                    context.NewsTags.RemoveRange(newsArticleToDelete.NewsTags);
                }

                // Xóa NewsArticle
                context.NewsArticles.Remove(newsArticleToDelete);
                context.SaveChanges();
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

        public async Task<List<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.NewsArticles
                .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

    }
}
