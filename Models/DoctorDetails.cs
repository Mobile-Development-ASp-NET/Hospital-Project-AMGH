using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hospital_Project.Models
{
    public class DoctorDetails
    {
            [Key]
            public int DrId { get; set; }
            public string DrFname { get; set; }
            public string DrLname { get; set; }
            public string DrEmail { get; set; }
            public string DrBio { get; set; }
            public string DrStudies { get; set; }
            public string DrPosition { get; set; }

            // A Doctor belongs to one department but a department can have many doctors.
            [ForeignKey("Department")]
            public int DepartmentId { get; set; }
            public virtual Department Department { get; set; }
            public ICollection<Admission> Admissions { get; set; }
            public ICollection<Feedback> Feedbacks { get; set; }

    }
    public class DoctorDetailDto
    {
        public int DrId { get; set; }
        public string DrFname { get; set; }
        public string DrLname { get; set; }
        public string DrEmail { get; set; }
        public string DrBio { get; set; }
        public string DrStudies { get; set; }
        public string DrPosition { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }

}