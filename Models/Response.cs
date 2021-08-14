using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_Project.Models
{
    public class Response
    {
        [Key]
        public int ResponseID { get; set; }
        public string Answer { get; set; }

        [ForeignKey("Question")]
        public int QuestionID { get; set; }
        public virtual Question Question { get; set; }

    }

    public class ResponseDto
    {
        public int QuestionID { get; set; }
        public string QuestionTitle { get; set; }
        public int ResponseID { get; set; }
        public string Answer { get; set; }
    }
}