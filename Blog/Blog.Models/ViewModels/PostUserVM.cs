using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Models.ViewModels
{
	public class PostUserVM
	{
		public Post Post { get; set; }

		[ValidateNever]
		public User User { get; set; }

		[ValidateNever]
		public String snippet { get; set; }

		public int MaxPages { get; set; } 
		public int CurrentPage { get; set; } 
        public int? TagEnable { get; set; }

	}
}
