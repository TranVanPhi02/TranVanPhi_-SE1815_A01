using BusinessObjects;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public void AddCategory(Category category)=>CategoryDAO.AddCategory(category);

        public void DeleteCategory(Category category)=>CategoryDAO.DeleteCategory(category);

        public Category GetCategoryById(int id)=>CategoryDAO.GetCategoryById(id);
        public List<Category> GetCategorys()=>CategoryDAO.GetCategorys();

        public void UpdateCategory(Category category)=>CategoryDAO.UpdateCategory(category);
    }
}
