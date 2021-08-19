using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital_Project.Models.ViewModels
{
    public class AdmissionList
    {

        //To check whether the user is admin or not
        public bool IsAdmin { get; set; }
        //contains all the greeting card details
        public IEnumerable<AdmissionDto> Admissions { get; set; }
    }
}