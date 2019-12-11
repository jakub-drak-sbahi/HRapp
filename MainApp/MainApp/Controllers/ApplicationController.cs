using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainApp.EntityFramework;
using MainApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainApp.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly DataContext _context;

        public ApplicationController(DataContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Index([FromQuery(Name = "search")] string searchString)
        {
            int n;
            int.TryParse(searchString, out n);

            if (string.IsNullOrEmpty(searchString) || n<=0)
                return View(await _context.JobApplications.ToListAsync());

            
            List<Application> searchResult = await _context
                .JobApplications
                .Where(o => o.OfferId==n)
                .ToListAsync();

            return View(searchResult);
        }

        public async Task<ActionResult> Create(int id)
        {
            var model = new Application() { OfferId = id };
            return View(model);
        }

        [HttpPost]  
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Application model, int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Application application = new Application
            {
                OfferId = id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                EmailAddress = model.EmailAddress,
                ContactAgreement = model.ContactAgreement,
                CvUrl = model.CvUrl
            };

            await _context.JobApplications.AddAsync(application);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest($"id should not be null");
            }

            _context.JobApplications.Remove(new Application() { Id = id.Value });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
