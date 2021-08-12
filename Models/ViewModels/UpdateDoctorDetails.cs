using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital_Project.Models.ViewModels
{
    public class UpdateDoctorDetails
    {
        public DoctorDetailDto selectedDoctor { get; set; }

        // all departments to choose

        public IEnumerable<DepartmentDto> departmentOptions { get; set; }
    }
}