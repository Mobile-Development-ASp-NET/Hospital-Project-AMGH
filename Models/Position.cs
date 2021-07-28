using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Hospital_Project.Models
{
    public class Position
    {
        [Key]
        public int PositionID { get; set; }
        public string PositionJob { get; set; }
        public string PositionDescription { get; set; }
        public DateTime PositionPostedDate { get; set; }
        public DateTime ApplicationDeadLine { get; set; }

        /*
         * Add a Foreign Key to Departments when that table is completed
        /*[ForeignKey("Department")]
        public int DepartmentID { get; set; }
        public virtual Department Departments { get; set; }
         */
    }

    public class PositionDto
    {
        public int PositionID { get; set; }
        public string PositionJob { get; set; }
        public string PositionDescription { get; set; }
        public DateTime PositionPostedDate { get; set; }
        public DateTime ApplicationDeadLine { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }

    }
}