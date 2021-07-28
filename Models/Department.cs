using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Hospital_Project.Models
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentDescription { get; set; }
    }

    public class DepartmentDto
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentDescription { get; set; }
    }
}