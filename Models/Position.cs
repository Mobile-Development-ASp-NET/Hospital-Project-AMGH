using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace Hospital_Project.Models
{
    public class Position
    {
        [Key]
        public int PositionID { get; set; }

        [Required]
        public string PositionJob { get; set; }

        [Required]
        public string PositionDescription { get; set; }

        [Required]
        public DateTime PositionPostedDate { get; set; }

        [Required]
        public DateTime ApplicationDeadLine { get; set; }

        /*
         * Add a Foreign Key to Departments when that table is completed
         * Since a position can have multiple departments while a department can have one position.
         */

        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
        public virtual Department Department { get; set; }

    }

    public class PositionDto
    {
        public int PositionID { get; set; }

        [Required(ErrorMessage = "Please Enter The Job Name.")]
        public string PositionJob { get; set; }

        [Required(ErrorMessage = "Please Enter The Job Description.")]
        public string PositionDescription { get; set; }

        [Required(ErrorMessage = "Please Enter The Posted Date For The Job.")]
        public DateTime PositionPostedDate { get; set; }

        [Required(ErrorMessage = "Please Enter The Deadline For The Job.")]
        public DateTime ApplicationDeadLine { get; set; }


        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }

    }
}