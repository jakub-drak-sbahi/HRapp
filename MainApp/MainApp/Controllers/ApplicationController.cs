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
                return View("IndexHR", searchResult);
            }
            else if (role == Role.CANDIDATE)
            {
                string email = AuthorizationTools.GetEmail(User);
                Candidate us = _context.Candidates.Where(c => c.EmailAddress == email).First();
                searchResult = searchResult.Where(a => a.Candidate == us).ToList();
                return View("IndexCandidate", searchResult);
            }
            return View("IndexAdmin", searchResult);
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
                FirstName = candidate.FirstName,
                LastName = candidate.LastName,
                PhoneNumber = candidate.PhoneNumber,
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
            model.State = "Pending";
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
                JobOffer = model.JobOffer,
                State = model.State
            };

            await _context.JobApplications.AddAsync(application);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest($"id shouldn't not be null");
            }
            string email = AuthorizationTools.GetEmail(User);
            Candidate us = _context.Candidates.Where(h => h.EmailAddress == email).FirstOrDefault();
            var app = await _context.JobApplications.FirstOrDefaultAsync(x => x.Id == id.Value);
            if (us == null || app == null || app.State!="Pending" || us.Id != app.Candidate.Id)
                return new UnauthorizedResult();

            return View(app);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit(Application model)
        {
            string email = AuthorizationTools.GetEmail(User);
            Candidate us = _context.Candidates.Where(h => h.EmailAddress == email).FirstOrDefault();
            var app = await _context.JobApplications.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (us == null || app == null || app.State != "Pending" || us.Id != app.Candidate.Id)
                return new UnauthorizedResult();

            if (!ModelState.IsValid)
            {
                return View();
            }
            app.FirstName = model.FirstName;
            app.LastName = model.LastName;
            app.PhoneNumber = model.PhoneNumber;
            app.EmailAddress = model.EmailAddress;
            app.ContactAgreement = model.ContactAgreement;
            app.CvUrl = model.CvUrl;
            _context.Update(app);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = model.Id });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            Role role = await AuthorizationTools.GetRoleAsync(User, _context);
            ViewData.Add("role", role);
            string email = AuthorizationTools.GetEmail(User);

            if (role == Role.ADMIN)
                return new UnauthorizedResult();

            Application app = _context.JobApplications
                .Include(x => x.JobOffer)
                .Include(x => x.Candidate)
                .Include(x => x.JobOffer.HR)
                .Include(x => x.JobOffer.HR.Company)
                .Where(a => a.Id == id)
                .FirstOrDefault();
            if (app == null)
                return new NotFoundResult();

            if(role == Role.HR)
            {
                HR us = _context.HRs.Where(c => c.EmailAddress == email).FirstOrDefault();
                if (us == null || us.Id != app.JobOffer.HR.Id)
                    return new UnauthorizedResult();
                return View("DetailsHR", app);
            }
            else
            {
                Candidate us = _context.Candidates.Where(c => c.EmailAddress == email).FirstOrDefault();
                if (us == null || us.Id != app.Candidate.Id)
                    return new UnauthorizedResult();

                return View("DetailsCandidate", app);
            }
        }

        [HttpPost, ActionName("Accept")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> AcceptConfirmed(int id)
        {
            Role role = await AuthorizationTools.GetRoleAsync(User, _context);
            ViewData.Add("role", role);
            string email = AuthorizationTools.GetEmail(User);
            if (role != Role.HR)
                return new UnauthorizedResult();
            Application app = _context.JobApplications
                .Include(x => x.JobOffer)
                .Include(x => x.Candidate)
                .Include(x => x.JobOffer.HR)
                .Include(x => x.JobOffer.HR.Company)
                .Where(a => a.Id == id)
                .FirstOrDefault();
            if (app == null)
            {
                return NotFound();
            }
            HR us = _context.HRs.Where(c => c.EmailAddress == email).FirstOrDefault();
            if (us == null || app.State != "Pending" || us.Id != app.JobOffer.HR.Id)
                return new UnauthorizedResult();
            app.State = "Accepted";
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Reject")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> RejectConfirmed(int id)
        {
            Role role = await AuthorizationTools.GetRoleAsync(User, _context);
            ViewData.Add("role", role);
            string email = AuthorizationTools.GetEmail(User);
            if (role != Role.HR)
                return new UnauthorizedResult();
            Application app = _context.JobApplications
                .Include(x => x.JobOffer)
                .Include(x => x.Candidate)
                .Include(x => x.JobOffer.HR)
                .Include(x => x.JobOffer.HR.Company)
                .Where(a => a.Id == id)
                .FirstOrDefault();
            if (app == null)
            {
                return NotFound();
            }
            HR us = _context.HRs.Where(c => c.EmailAddress == email).FirstOrDefault();
            if (us == null || app.State != "Pending" || us.Id != app.JobOffer.HR.Id)
                return new UnauthorizedResult();
            app.State = "Rejected";
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            Role role = await AuthorizationTools.GetRoleAsync(User, _context);
            ViewData.Add("role", role);
            string email = AuthorizationTools.GetEmail(User);
            if (role != Role.CANDIDATE)
                return new UnauthorizedResult();
            if (id == null)
            {
                return NotFound();
            }
            Application app = _context.JobApplications
                .Include(x => x.JobOffer)
                .Include(x => x.Candidate)
                .Include(x => x.JobOffer.HR)
                .Include(x => x.JobOffer.HR.Company)
                .Where(a => a.Id == id)
                .FirstOrDefault();
            if (app == null)
            {
                return NotFound();
            }
            Candidate us = _context.Candidates.Where(c => c.EmailAddress == email).FirstOrDefault();
            if (us == null || app.State != "Pending" || us.Id != app.Candidate.Id)
                return new UnauthorizedResult();
            return View(app);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Role role = await AuthorizationTools.GetRoleAsync(User, _context);
            ViewData.Add("role", role);
            string email = AuthorizationTools.GetEmail(User);
            if (role != Role.CANDIDATE)
                return new UnauthorizedResult();
            Application app = _context.JobApplications
                .Include(x => x.JobOffer)
                .Include(x => x.Candidate)
                .Include(x => x.JobOffer.HR)
                .Include(x => x.JobOffer.HR.Company)
                .Where(a => a.Id == id)
                .FirstOrDefault();
            if (app == null)
            {
                return NotFound();
            }
            Candidate us = _context.Candidates.Where(c => c.EmailAddress == email).FirstOrDefault();
            if (us == null || app.State != "Pending" || us.Id != app.Candidate.Id)
                return new UnauthorizedResult();
            _context.JobApplications.Remove(app);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
