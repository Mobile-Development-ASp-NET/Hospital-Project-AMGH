using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital_Project.Models.ViewModels
{
    public class UpdateApplication
    {
        public ApplicationDto SelectedApplication { get; set; }
        public IEnumerable<PositionDto> PositionOptions { get; set; }
    }
}