using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainApp.Models
{
    public class HR
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public virtual Company Company { get; set; }
        public virtual int CompanyId { get; set; }
        public List<JobOffer> JobOffers { get; set; } = new List<JobOffer>();
    }
}
