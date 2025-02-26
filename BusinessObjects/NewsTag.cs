using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class NewsTag
    {
        public string? NewsArticleID { get; set; }
        public int TagID { get; set; }

        public virtual NewsArticle? NewsArticle { get; set; } = null;  // Khởi tạo null để tránh lỗi
        public virtual Tag? Tag { get; set; } = null;  // Khởi tạo null để tránh lỗi
    }

}
