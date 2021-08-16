using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital_Project.Models.ViewModels
{
    public class UpdateAdmission
    {
        public AdmissionDto SelectedAdmission { get; set; }

        // all doctors to choose from when updating this admission

        public IEnumerable<DoctorDetailDto> DoctorOptions { get; set; }

        public IEnumerable<ApplicationUser> UserOptions { get; set; }
    }
}