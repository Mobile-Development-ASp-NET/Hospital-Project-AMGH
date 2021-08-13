using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_Project.Models
{
    public class GreetingCard
    {
        [Key]
        public int CardId { get; set; }
        [Required]
        public string SenderFirstName { get; set; }
        [Required]
        public string SenderLastName { get; set; }
        [Required]
        [AllowHtml]
        public string CardMessage { get; set; }
        public string CardType { get; set; }
        public bool CardHasPic { get; set; }
        public string PicExtension { get; set; }
        //a card belongs to one patient
        //a patient can recieve many cards
        [ForeignKey("Admissions")]
        public int AdmissionId { get; set; }
        public virtual Admission Admissions { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
    public class GreetingCardDto
    {
        public int CardId { get; set; }
        [Required(ErrorMessage = "Please Enter a sender first Name.")]
        public string SenderFirstName { get; set; }
        [Required(ErrorMessage = "Please Enter a sender last Name.")]
        public string SenderLastName { get; set; }
        public string CardType { get; set; }
        [Required(ErrorMessage = "Please Enter a Message.")]
        [AllowHtml]
        public string CardMessage { get; set; }
        public bool CardHasPic { get; set; }
        public string PicExtension { get; set; }
        public int AdmissionId { get; set; }
        public string Room { get; set; }
        public string Bed { get; set; }
        public string UserId { get; set; }

    }
}
    