using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital_Project.Models.ViewModels
{
    public class UpdateGreetingCard
    {
        //This viewmodel is a class which stores information that we need to present to /GreetingCard/Update/{}

        //the existing card information

        public GreetingCardDto SelectedCard { get; set; }

        // all admissions to choose from when updating this card

        public IEnumerable<AdmissionDto> AdmissionOptions { get; set; }
    }
}