using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainApp.Models
{
    public class JobOffer
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
    }
}
