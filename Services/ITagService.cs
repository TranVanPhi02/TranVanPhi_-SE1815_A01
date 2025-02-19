using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ITagService
    {
        List<Tag> GetTags();
        void AddTag(Tag tag);
        void UpdateTag(Tag tag);
        void DeleteTag(Tag tag);
        Tag GetTagById(int id);
    }
}
