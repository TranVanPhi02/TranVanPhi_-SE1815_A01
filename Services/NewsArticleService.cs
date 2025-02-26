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
        private readonly INewsArticleRepository _newsArticleRepository;
        public NewsArticleService(INewsArticleRepository newsArticleRepository)
        {
            _newsArticleRepository = newsArticleRepository;
        }
        public void AddNewsArticle(NewsArticle newsArticle)
        {
            _newsArticleRepository.AddNewsArticle(newsArticle);
        }

        public void DeleteNewsArticle(string id)
        {
            _newsArticleRepository.DeleteNewsArticle(id);
        }

        public NewsArticle GetNewsArticleById(int id)
        {
            return _newsArticleRepository.GetNewsArticleById(id);
        }

        public List<NewsArticle> GetNewsArticles()
        {
            return _newsArticleRepository.GetNewsArticles();
        }

        public async Task<List<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            // Kiểm tra xem repository có được khởi tạo không
            if (_newsArticleRepository == null)
            {
                throw new Exception("NewsArticleRepository is not initialized.");
            }

            // Kiểm tra xem dữ liệu có hợp lệ không
            var result = await _newsArticleRepository.GetNewsByDateRangeAsync(startDate, endDate);

            if (result == null || !result.Any())
            {
                // Trả về danh sách rỗng nếu không có kết quả
                return new List<NewsArticle>();
            }

            return result;
        }


        public void UpdateNewsArticle(NewsArticle newsArticle)
        {
            _newsArticleRepository.UpdateNewsArticle(newsArticle);
        }
    }
}
