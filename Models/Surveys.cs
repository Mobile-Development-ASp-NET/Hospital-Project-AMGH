using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_Project.Models
{
    public class Surveys
    {
        [Key]
        public int SurveyID { get; set; }
        public string SurveyTitle { get; set; }
        public string SurveyDescription { get; set; }

        public ICollection<Questions> Questions { get; set; }
    }
}