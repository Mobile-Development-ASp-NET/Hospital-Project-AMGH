using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Hospital_Project.Models
{
    public class Survey
    {
        [Key]
        public int SurveyID { get; set; }
        [Required]
        public string SurveyTitle { get; set; }
        [AllowHtml]
        public string SurveyDescription { get; set; }

        public ICollection<Question> Questions { get; set; }
    }

    public class SurveyDto
    {
        public int SurveyID { get; set; }
        [Required(ErrorMessage ="Please enter a title of the survey")]
        public string SurveyTitle { get; set; }
        [Required(ErrorMessage ="Please enter the description of the survey")]

        [AllowHtml]
        public string SurveyDescription { get; set; }
    }
}