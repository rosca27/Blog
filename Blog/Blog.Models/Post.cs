using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        
        [ValidateNever]
        public int Likes { get; set; }
        [ValidateNever]
        public string Picture { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public DateTime UpdateDate { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [ValidateNever]
        public List<Comment> Comments { get; set; }
        [ValidateNever]
        public User User { get; set; }

		[ValidateNever]
		public virtual ICollection<Tag> Tags { get; set; }
		public Post()
        {
            this.Likes = 0;
            this.Tags= new HashSet<Tag>();
        }

		
	}
}
