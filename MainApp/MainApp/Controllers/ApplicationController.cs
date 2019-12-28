using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainApp.Authorization;
using MainApp.EntityFramework;
using MainApp.Models;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index([FromQuery(Name = "search")] string searchString)
        {
            Role role = await AuthorizationTools.GetRoleAsync(User, _context);
            if (string.IsNullOrEmpty(searchString))
                return View(await _context.JobApplications.ToListAsync());

            List<Application> searchResult = await _context
                .JobApplications
                .Where(o => o.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
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
        [Authorize]
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
    }
}
