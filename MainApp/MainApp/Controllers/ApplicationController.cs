using System;
using System.Collections.Generic;
using MainApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace MainApp.Controllers
{
    public class ApplicationController:Controller
    {
        private static List<Application> _applications = new List<Application>
        {
            new Application{Id=1, OfferId=1, FirstName="John", LastName="Black", PhoneNumber="123456789", EmailAddress="john@black.com", ContactAgreement=true, CvUrl="url1" },
            new Application{Id=2, OfferId=1, FirstName="Kate", LastName="Green", PhoneNumber="123222789", EmailAddress="kate@green.com", ContactAgreement=true, CvUrl="url2" },
            new Application{Id=3, OfferId=2, FirstName="Joe", LastName="Blue", PhoneNumber="123111789", EmailAddress="joe@blue.com", ContactAgreement=false, CvUrl="url3" },
        };

        public IActionResult Index()
        {
            return View("Index", _applications);
        }
    }
}
