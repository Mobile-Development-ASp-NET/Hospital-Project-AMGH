using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hospital_Project.Models
{
    public class Application
    {
        [Key]
        public int ApplicationID { get; set; }

        [Required]
        public string ApplicationName { get; set; }

        [Required]
        public DateTime ApplicationDOB { get; set; }

        [Required]
        public string ApplicationEmail { get; set; }

        public Boolean ApplicationCriminalRecord { get; set; }

        [Required]
        public string ApplicationStatus { get; set; }


        //An application belongs to one position but a position can have multiple applications
        [ForeignKey("Position")]
        public int PositionID { get; set;}
        public virtual Position Position { get; set; }
    }

    public class ApplicationDto
    {
        public int ApplicationID { get; set; }

        [Required(ErrorMessage = "Please Enter a Name.")]
        public string ApplicationName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Date Of Birth.")]
        public DateTime ApplicationDOB { get; set; }

        [Required(ErrorMessage = "Please Enter an Email.")]
        public string ApplicationEmail { get; set; }


        public Boolean ApplicationCriminalRecord { get; set; }

        [Required(ErrorMessage = "Please Update the Application Status.")]
        public string ApplicationStatus { get; set; }


        public int PositionID { get; set; }
        public string PositionJob { get; set; }

    }
}