using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital_Project.Models.ViewModels
{
    public class DetailsSurvey
    {
        public SurveyDto selectedSurvey { get; set; }
        public List<QuestionDto> relatedquestions { get; set; }
    }
}