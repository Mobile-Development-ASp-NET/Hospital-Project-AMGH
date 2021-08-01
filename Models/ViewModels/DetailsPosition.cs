using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital_Project.Models.ViewModels
{
    public class DetailsPosition
    {
        public PositionDto SelectedPositions { get; set; }
        public IEnumerable<ApplicationDto> RelatedApplication { get; set; }
        public IEnumerable<DepartmentDto> RelatedDepartment { get; set; }
    }
}