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
        public string ApplicationName { get; set; }
        public DateTime ApplicationDOB { get; set; }
        public string ApplicationEmail { get; set; }
        public Boolean ApplicationCriminalRecord { get; set; }
        public string ApplicationStatus { get; set; }


        //An application belongs to one position but a position can have multiple applications
        [ForeignKey("Position")]
        public int PositionID { get; set;}
        public virtual Position Position { get; set; }
    }

    public class ApplicationDto
    {
        public int ApplicationID { get; set; }
        public string ApplicationName { get; set; }
        public DateTime ApplicationDOB { get; set; }
        public string ApplicationEmail { get; set; }
        public Boolean ApplicationCriminalRecord { get; set; }
        public string ApplicationStatus { get; set; }
        public int PositionID { get; set; }
        public string PositionJob { get; set; }

    }
}