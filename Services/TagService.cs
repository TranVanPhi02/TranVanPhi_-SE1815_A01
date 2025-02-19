using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository iTagRepository;
        public void AddTag(Tag tag)
        {
            iTagRepository.AddTag(tag);
        }

        public void DeleteTag(Tag tag)
        {
            iTagRepository.DeleteTag(tag);
        }

        public Tag GetTagById(int id)
        {
           return iTagRepository.GetTagById(id);    
        }

        public List<Tag> GetTags()
        {
           return iTagRepository.GetTags();
        }

        public void UpdateTag(Tag tag)
        {
           iTagRepository.UpdateTag(tag);
        }
    }
}
