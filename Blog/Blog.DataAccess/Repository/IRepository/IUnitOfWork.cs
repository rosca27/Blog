using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IPostRepository Post { get; }
        ITagRepository Tag { get; }
        ICommentRepository Comment { get; }

        void Save();

    }
}
