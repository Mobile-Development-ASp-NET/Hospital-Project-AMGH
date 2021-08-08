using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital_Project.Models.ViewModels
{
    public class DetailsFeedback
    {
        public FeedbackDto SelectedFeedback { get; set; }

        public IEnumerable<DoctorDetailDto> RelatedDoctors { get; set; }
    }
}