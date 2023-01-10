using Blog.DataAccess.Data;
using Blog.DataAccess.Repository.IRepository;
using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DataAccess.Repository
{
    public class TagRepository : Repository<Tag>,  ITagRepository
    {
        private ApplicationDbContext _db;
        public TagRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Tag obj)
        {
            _db.Tags.Update(obj);
        }
    }
}

