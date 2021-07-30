using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital_Project.Models.ViewModels
{
    public class AdmissionDetails
    {
        public AdmissionDto SelectedAdmission { get; set; }
        //cards received by this patient
        public IEnumerable<GreetingCardDto> RelatedCards { get; set; }
    }
}