using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital_Project.Models.ViewModels
{
    public class UpdatePosition
    {
        public PositionDto SelectedPosition { get; set; }
        public IEnumerable<DepartmentDto> DepartmentOptions { get; set; }
    }
}