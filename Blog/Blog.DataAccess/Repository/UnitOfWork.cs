using Blog.DataAccess.Data;
using Blog.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db) 
        {
            _db = db;
            Comment = new CommentRepository(_db);
            Tag = new TagRepository(_db);
            Post = new PostRepository(_db);

        }

        public IPostRepository Post { get; private set; }
        public ITagRepository Tag { get; private set; }
        public ICommentRepository Comment { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
