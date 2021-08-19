using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_Project.Models
{
    public class Admission
    {
        [Key]
        public int AdmissionId { get; set; }
        public string Room { get; set; }
        public string Bed { get; set; }

        //an admitted  patient belongs to one doctor
        //a doctor can have many patients
        [ForeignKey("DoctorDetails")]
        public int DrId { get; set;}
        public virtual DoctorDetails DoctorDetails { get; set; }

        //an admitted patient belongs to the users
        //users can have many admitted patients
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public ICollection<GreetingCard> GreetingCards { get; set; }
    }
    public class AdmissionDto
    {
        public int AdmissionId { get; set; }
        public string Room { get; set; }
        public string Bed { get; set; }
        public int DrId { get; set; }
        public string DrFname { get; set; }
        public string DrLname { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

    }
}