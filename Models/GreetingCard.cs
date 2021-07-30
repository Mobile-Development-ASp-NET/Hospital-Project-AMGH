using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_Project.Models
{
    public class GreetingCard
    {
        [Key]
        public int CardId { get; set; }
        public string SenderFirstName { get; set; }
        public string SenderLastName { get; set; }
        public string CardMessage { get; set; }
        public string CardType { get; set; }
        public bool CardHasPic { get; set;}
        public string PicExtension { get; set; }
        //a card belongs to one patient
        //a patient can recieve many cards
        [ForeignKey("Admissions")]
        public int AdmissionId { get; set; }
        public virtual Admission Admissions { get; set; }

    }
    public class GreetingCardDto
    {
        public int CardId { get; set; }
        public string SenderFirstName { get; set; }
        public string SenderLastName { get; set; }
        public string CardType { get; set; }
        public string CardMessage { get; set; }
        public bool CardHasPic { get; set; }
        public string PicExtension { get; set; }
        public int AdmissionId { get; set;}
        public string Room { get; set;}
        public string Bed { get; set; }

    }
}