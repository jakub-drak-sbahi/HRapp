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
            ViewData.Add("role", role);
            List<Application> searchResult;

            if (string.IsNullOrEmpty(searchString))
                searchResult = await _context.JobApplications
                     .Include(x => x.JobOffer)
                     .Include(x => x.JobOffer.HR)
                     .Include(x => x.JobOffer.HR.Company)
                     .ToListAsync();
            else
            {
                searchResult = await _context
                    .JobApplications
                    .Include(x => x.JobOffer)
                    .Include(x => x.JobOffer.HR)
                    .Include(x => x.JobOffer.HR.Company)
                    .Where(o => o.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToListAsync();
            }
            if (role == Role.HR)
            {
                string email = AuthorizationTools.GetEmail(User);
                HR us = _context.HRs.Where(h => h.EmailAddress == email).First();
                searchResult = searchResult.Where(a => a.JobOffer.HR == us).ToList();
            }
            else if (role == Role.CANDIDATE)
            {
                string email = AuthorizationTools.GetEmail(User);
                Candidate us = _context.Candidates.Where(c => c.EmailAddress == email).First();
                searchResult = searchResult.Where(a => a.Candidate == us).ToList();
            }
            return View(searchResult);
        }

        public async Task<ActionResult> Create(int id)
        {
            Role role = await AuthorizationTools.GetRoleAsync(User, _context);
            ViewData.Add("role", role);

            if (role != Role.CANDIDATE)
                return new UnauthorizedResult();
            JobOffer offer = _context.JobOffers.Where(o => o.Id == id).First();
            string email = AuthorizationTools.GetEmail(User);
            Candidate candidate = _context.Candidates.Where(c => c.EmailAddress == email).First();
            var model = new Application()
            {
                CvUrl = "TODO",
                EmailAddress = email,
                JobOffer = offer,
                Candidate = candidate,
                OfferId = id
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(Application model, int id)
        {
            Role role = await AuthorizationTools.GetRoleAsync(User, _context);
            ViewData.Add("role", role);

            if (role != Role.CANDIDATE)
                return new UnauthorizedResult();
            JobOffer offer = _context.JobOffers.Where(o => o.Id == id).First();
            string email = AuthorizationTools.GetEmail(User);
            Candidate candidate = _context.Candidates.Where(c => c.EmailAddress == email).First();
            model.JobOffer = offer;
            model.Candidate = candidate;
            model.OfferId = offer.Id;
            model.CvUrl = "TODO";

            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}

            Application application = new Application
            {
                OfferId = id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                EmailAddress = model.EmailAddress,
                ContactAgreement = model.ContactAgreement,
                CvUrl = model.CvUrl,
                Candidate = model.Candidate,
                JobOffer = model.JobOffer
            };

            await _context.JobApplications.AddAsync(application);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
