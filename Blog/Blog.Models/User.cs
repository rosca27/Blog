using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class User : IdentityUser
    {   
        [MaxLength(50)]
        public string FirstName { get; set; }

        
        [MaxLength(30)]
        public string LastName { get; set; }

        [MaxLength(11)]
        public string PhoneNumber { get; set; }

        public String Address { get; set; }

        public string Description { get; set; }


        public DateTime CreateDate { get; set; } = DateTime.Now;

        public DateTime UpdateDate { get; set; }

        //Comment
        public List<Comment> Comments { get; set; }

        public List<Post> Post { get; set; }   

        
    }
}
