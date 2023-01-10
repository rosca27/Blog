using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(40)]
        public string Name { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public DateTime UpdateDate { get; set; }

        [ValidateNever]
		public virtual ICollection<Post> Posts { get; set; }

        public Tag()
        {
            this.Posts= new HashSet<Post>();
        }
	}
}
