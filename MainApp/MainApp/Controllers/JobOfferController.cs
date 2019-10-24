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
            new JobOffer{Id=1, JobTitle="Backend Developer", City="Radzyń Podlaski", Company="Spółdzielnia mleczarska SPOMLEK", Description="Opis w budowie", ExpirationDate=DateTime.Now },
            new JobOffer{Id=2, JobTitle="Frontend Developer" },
            new JobOffer{Id=3, JobTitle="Korektor", City="Rzabików", Company="PGA", Description="Zatródnimy korektora", ExpirationDate=DateTime.MaxValue},
            new JobOffer{Id=4, JobTitle="Manager" },
            new JobOffer{Id=5, JobTitle="Teacher" },
            new JobOffer{Id=6, JobTitle="Cook" },
            new JobOffer{Id=7, JobTitle="Volkswagen Passat B7 2013 przebieg 87000", City="Pszczyna", Company="PGA", Description="Wymieniony olej w skrzyni biegów, nowy rozrząd, nowe opony. Samochód jest w stanie bardzo dobrym i nie wymaga żadnego wkładu finansowego. Idealny dla rodzin a także idealny na górskie wypady.Samochód godny polecenia. Cena: 56.500,00 brutto", ExpirationDate=DateTime.MaxValue}
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
