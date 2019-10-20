using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MainApp.Models;

namespace MainApp.Controllers
{
    public class JobOfferController : Controller
    {
        private static List<JobOffer> _jobOffers = new List<JobOffer>
        {
            new JobOffer{Id=1, JobTitle="Backend Developer" },
            new JobOffer{Id=2, JobTitle="Frontend Developer" },
            new JobOffer{Id=3, JobTitle="Manager" },
            new JobOffer{Id=4, JobTitle="Teacher" },
            new JobOffer{Id=5, JobTitle="Cook" }
        };
        public IActionResult Index()
        {
            return View("Index", _jobOffers);
        }
        public IActionResult Details(int id)
        {
            if (id > _jobOffers.Count || id < 1)
                return new NotFoundResult();
            var model = _jobOffers[id-1];
            return View("Details", model);
        }
        
    }
}
