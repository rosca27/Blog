using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DataAccess.Repository.IRepository
{
    public interface ITagRepository : IRepository<Tag>
    {
        void Update(Tag obj);
    }
}
