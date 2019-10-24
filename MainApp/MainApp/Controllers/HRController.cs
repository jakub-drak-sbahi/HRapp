using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace MainApp.Controllers
{
    public class HRController : Controller
    {
        private static List<HR> __HRs = new List<HR>
        {
            new HR{Id=1, FirstName="Żaneta", Surname="Rybka", Company="Sklep wędkarski KARAŚ Warszawa" },
            new HR{Id=2, FirstName="Roksana", Surname="Koza", Company="Accenture" },
            new HR{Id=3, FirstName="Jeremi", Surname="Wiśniowiecki", Company="RON"}
        };
        public IActionResult Index()
        {
            return View("Index", __HRs);
        }
    }
}