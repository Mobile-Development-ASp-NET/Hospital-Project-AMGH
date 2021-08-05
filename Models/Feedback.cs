using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hospital_Project.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }

        // A Feedback belongs to one Patient. A Patient can have many Feedbacks.
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        // A Feedback belongs to one Doctor but a Doctor can have many Feedbacks.
        [ForeignKey("DoctorDetails")]
        public int DrId { get; set; }
        public virtual DoctorDetails DoctorDetails { get; set; }

        public string FeedbackContent { get; set; }

        public DateTime FeedbackDate { get; set; }
    }

    public class FeedbackDto

    {
        public int FeedbackId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int DrId { get; set; }
        public string DrFname { get; set; }
        public string DrLname { get; set; }
        public string FeedbackContent { get; set; }
        public DateTime FeedbackDate { get; set; }

    }
}