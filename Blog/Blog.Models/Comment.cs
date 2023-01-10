using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public DateTime UpdateDate { get; set; }

        [ValidateNever]
        public Post Post { get; set; }

		[ValidateNever]
		public User User { get; set; }

        [ValidateNever]
        public virtual IEnumerable<Comment> Comments { get; set; }

        public Comment()
        {
            Comments = new List<Comment>();
        }

    }
}
