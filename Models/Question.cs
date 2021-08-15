using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Hospital_Project.Models
{
    public class Question
    {
        [Key]
        public int QuestionID { get; set; }
        [Required]
        public string QuestionTitle { get; set; }
        [AllowHtml]
        public string QuestionyDescription { get; set; }

        public ICollection<Survey> Surveys { get; set; }
    }

    public class QuestionDto
    {
        public int QuestionID { get; set; }
        [Required(ErrorMessage ="Please enter a question title")]
        public string QuestionTitle { get; set; }
        [AllowHtml]
        public string QuestionyDescription { get; set; }
    }
}