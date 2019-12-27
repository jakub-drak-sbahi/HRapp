using System;
using System.ComponentModel.DataAnnotations;

namespace MainApp.Models
{
    public class Application
    {
        public int Id { get; set; }
        [Display(Name = "Offer")]
        public int OfferId { get; set; }
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }
        [Required]
        [Display(Name = "Contact agreement")]
        public bool ContactAgreement { get; set; }
        [Required]
        public string CvUrl { get; set; }
        public Candidate Candidate { get; set; }
        public JobOffer JobOffer { get; set; }
    }
}
