using System;

namespace MainApp.Models
{
    public class Application
    {
        public int Id { get; set; }
        public int OfferId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public bool ContactAgreement { get; set; }
        public string CvUrl { get; set; }
    }
}
