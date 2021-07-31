using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital_Project.Models.ViewModels
{
    public class DetailsBlog
    {
        public BlogDto SelectedBlog { get; set; }
        public IEnumerable<SubscribedUserDto> ApprovedUsers { get; set; }
    }
}