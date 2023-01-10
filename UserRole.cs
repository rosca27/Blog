using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Models
{
    internal class UserRole
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Role { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
