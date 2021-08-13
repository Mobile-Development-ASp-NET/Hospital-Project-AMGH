using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital_Project.Models.ViewModels
{
    /// <summary>
    /// view model for greeting card details
    /// </summary>
    public class GreetingCardDetails
    {
        //to check whether its admin or not
        public bool IsAdmin { get; set; }
        public GreetingCardDto SelectedCard { get; set; }
    }
}