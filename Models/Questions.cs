using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_Project.Models
{
    public class Questions
    {
        [Key]
        public int QuestionID { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionyDescription { get; set; }

        public ICollection<Surveys> Surveys { get; set; }
    }
}