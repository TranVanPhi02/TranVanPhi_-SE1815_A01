using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class CategoryDAO
    {
            public static List<Category> GetCategorys()
            {
                var listCategorys = new List<Category>();
                try
                {

                    using var db = new FunewsManagementContext();

                    listCategorys = db.Categories.ToList();
               
                }
                catch (Exception e) { }
                return listCategorys;
            }

            public static void AddCategory(Category category)
            {
                try
                {
                    using var context = new FunewsManagementContext();
                    context.Categories.Add(category); 
                    context.SaveChanges(); // Update Database
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

            }

            public static void UpdateCategory(Category category)
            {
                try
                {
                    using var context = new FunewsManagementContext();
                    context.Entry<Category>(category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

            }

        public static void DeleteCategory(Category category)
        {
            try
            {
                using var context = new FunewsManagementContext();

                var categoryInUse = context.NewsArticles.Any(na => na.CategoryId == category.CategoryId);

                if (categoryInUse)
                {
                    throw new Exception("Cannot delete this category because it is associated with one or more news articles.");
                }

                var categoryToDelete = context.Categories.SingleOrDefault(c => c.CategoryId == category.CategoryId);

                if (categoryToDelete == null)
                {
                    throw new Exception("Category not found.");
                }

                context.Categories.Remove(categoryToDelete);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception($"Error deleting category: {e.Message}");
            }
        }


        public static Category GetCategoryById(int id)
            {
                using var db = new FunewsManagementContext();
                return db.Categories.FirstOrDefault(c => c.CategoryId.Equals(id));
            }
        }
    }



