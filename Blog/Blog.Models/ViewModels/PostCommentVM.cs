using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Models.ViewModels
{
    public class PostCommentVM
    {
        public Post Post { get; set; }
        public Tag Tag { get; set; }
        public IEnumerable<Comment> CommentList { get; set;}

        public User User { get; set; }
    }
}
