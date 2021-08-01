using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hospital_Project.Models
{
    public class SubscribedUser
    {
        [Key]
        public int SubscribedUserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<Blog> Blogs { get; set; }
    }
    public class SubscribedUserDto
    {
        public int SubscribedUserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}