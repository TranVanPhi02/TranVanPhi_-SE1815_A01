using BusinessObjects;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class TagRepository : ITagRepository
    {
        public void AddTag(Tag tag)=>TagDAO.AddTag(tag);

        public void DeleteTag(Tag tag)=>TagDAO.DeleteTag(tag);

        public Tag GetTagById(int id)=>TagDAO.GetTagById(id);

        public List<Tag> GetTags()=>TagDAO.GetTags();
        public void UpdateTag(Tag tag)=>TagDAO.UpdateTag(tag);
    }
}
