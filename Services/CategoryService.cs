using BusinessObjects;
using Repositories;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository iCategoryRepository;
        public CategoryService()
        {
            iCategoryRepository = new CategoryRepository();
        }
        public void AddCategory(Category category)
        {
            iCategoryRepository.AddCategory(category);
        }

        public void DeleteCategory(Category category)
        {
           iCategoryRepository.DeleteCategory(category);
        }

        public Category GetCategoryById(int id)
        {
            return iCategoryRepository.GetCategoryById(id);
        }

        public List<Category> GetCategorys()
        {
            return iCategoryRepository.GetCategorys();
        }

        public void UpdateCategory(Category category)
        {
            iCategoryRepository.UpdateCategory(category);
        }
    }
}
