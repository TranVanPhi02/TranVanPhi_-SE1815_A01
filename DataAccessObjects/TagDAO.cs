using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class TagDAO
    {
        public static List<Tag> GetTags()
        {
            var listTags = new List<Tag>();
            try
            {

                using var db = new FunewsManagementContext();

                listTags = db.Tags.ToList();

            }
            catch (Exception e) { }
            return listTags;
        }

        public static void AddTag(Tag tag)
        {
            try
            {
                using var context = new FunewsManagementContext();
                context.Tags.Add(tag);
                context.SaveChanges(); // Update Database
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public static void UpdateTag(Tag tag)
        {
            try
            {
                using var context = new FunewsManagementContext();
                context.Entry<Tag>(tag).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public static void DeleteTag(Tag tag)
        {
            try
            {
                using var context = new FunewsManagementContext();
                var tagList = context.Tags.SingleOrDefault(t => t.TagId == tag.TagId);
                context.Tags.Remove(tagList);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static Tag GetTagById(int id)
        {
            using var db = new FunewsManagementContext();
            return db.Tags.FirstOrDefault(t => t.TagId.Equals(id));
        }
    }
}
