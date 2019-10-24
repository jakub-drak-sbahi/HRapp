using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MainApp.Models;

namespace MainApp.Controllers
{
    public class CandidateController : Controller
    {
        private static List<Candidate> _candidates = new List<Candidate>
        {
            new Candidate{Id=1, FirstName="John", LastName="Black", PhoneNumber="123456789", EmailAddress="john@black.com" },
            new Candidate{Id=2, FirstName="Kate", LastName="Green", PhoneNumber="123222789", EmailAddress="kate@green.com" },
            new Candidate{Id=3, FirstName="Joe", LastName="Blue", PhoneNumber="123111789", EmailAddress="joe@blue.com" },
        };

        public IActionResult Index()
        {
            return View("Index", _candidates);
        }
    }
}