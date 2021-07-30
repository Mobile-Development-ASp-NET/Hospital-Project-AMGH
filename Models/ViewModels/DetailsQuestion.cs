using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital_Project.Models.ViewModels
{
    public class DetailsQuestion
    {
        public QuestionDto selectedQuestion { get; set; }
        public List<SurveyDto> relatedSurveys { get; set; }
    }
}