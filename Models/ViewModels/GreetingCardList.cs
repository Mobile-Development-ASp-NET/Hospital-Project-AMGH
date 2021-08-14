using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital_Project.Models.ViewModels
{
    /// <summary>
    /// Class to list all the greeting card details
    /// </summary>
    public class GreetingCardList
    {
        //To check whether the user is admin or not
        public bool IsAdmin { get; set; }
        //contains all the greeting card details
        public IEnumerable<GreetingCardDto> GreetingCards { get; set; }
    }
}