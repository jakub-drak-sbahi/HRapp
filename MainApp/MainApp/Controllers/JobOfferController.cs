using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MainApp.Models;
using MainApp.EntityFramework;
using Microsoft.EntityFrameworkCore;
using MainApp.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace MainApp.Controllers
{
    public class JobOfferController : Controller
    {
        private readonly DataContext _context;

        public JobOfferController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index([FromQuery(Name = "search")] string searchString)
        {
            Role role = await AuthorizationTools.GetRoleAsync(User, _context);
            List<JobOffer> searchResult;
            if (string.IsNullOrEmpty(searchString))
            {
                searchResult = await _context.JobOffers.Include(x => x.Company).ToListAsync();
            }
            else
            {
                searchResult = await _context
                    .JobOffers.Include(x => x.Company)
                    .Where(o => o.JobTitle.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                    || o.Company.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToListAsync();
            }
            string email = AuthorizationTools.GetEmail(User);
            if (role == Role.HR)
            {
                JobOfferIndexHRView jobOfferIndexHRView = new JobOfferIndexHRView();
                jobOfferIndexHRView.Offers = searchResult;
                HR us = _context.HRs.Where(h => h.EmailAddress == email).First();
                jobOfferIndexHRView.HR = us;
                return View("IndexHR", jobOfferIndexHRView);
            }
            else if (role == Role.CANDIDATE)
            {
                JobOfferIndexCandidateView jobOfferIndexCandidateView = new JobOfferIndexCandidateView();
                jobOfferIndexCandidateView.Offers = searchResult;
                Candidate us = _context.Candidates.Where(c => c.EmailAddress == email).First();
                jobOfferIndexCandidateView.Candidate = us;
                return View("IndexCandidate", jobOfferIndexCandidateView);
            }
            //role == Role.ADMIN
            return View("IndexAdmin", searchResult);
        }
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            Role role = await AuthorizationTools.GetRoleAsync(User, _context);
            if (role == Role.CANDIDATE)
                return new UnauthorizedResult();
            if (id == null)
            {
                return BadRequest($"id shouldn't be null");
            }
            var offer = await _context.JobOffers.FirstOrDefaultAsync(x => x.Id == id.Value);
            if (offer == null)
            {
                return NotFound($"offer not found in DB");
            }
            if (role == Role.HR)
            {
                string email = AuthorizationTools.GetEmail(User);
                HR us = _context.HRs.Where(h => h.EmailAddress == email).First();
                if (us.Id != offer.HR.Id)
                    return new UnauthorizedResult();
            }

            return View(offer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(JobOffer model)
        {
            Role role = await AuthorizationTools.GetRoleAsync(User, _context);
            if (role == Role.CANDIDATE)
                return new UnauthorizedResult();

            if (!ModelState.IsValid)
            {
                return View();
            }

            var offer = await _context.JobOffers.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (role == Role.HR)
            {
                string email = AuthorizationTools.GetEmail(User);
                HR us = _context.HRs.Where(h => h.EmailAddress == email).First();
                if (us.Id != offer.HR.Id)
                    return new UnauthorizedResult();
            }
            offer.JobTitle = model.JobTitle;
            offer.Description = model.Description;
            _context.Update(offer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = model.Id });
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            Role role = await AuthorizationTools.GetRoleAsync(User, _context);
            if (role == Role.CANDIDATE)
                return new UnauthorizedResult();

            if (id == null)
            {
                return BadRequest($"id should not be null");
            }
            var offer = await _context.JobOffers.FirstOrDefaultAsync(x => x.Id == id.Value);
            if (role == Role.HR)
            {
                string email = AuthorizationTools.GetEmail(User);
                HR us = _context.HRs.Where(h => h.EmailAddress == email).First();
                if (us.Id != offer.HR.Id)
                    return new UnauthorizedResult();
            }
            _context.JobOffers.Remove(new JobOffer() { Id = id.Value });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Create()
        {
            var model = new JobOfferCreateView
            {
                Companies = await _context.Companies.ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(JobOfferCreateView model)
        {
            Role role = await AuthorizationTools.GetRoleAsync(User, _context);
            if (role == Role.CANDIDATE)
                return new UnauthorizedResult();

            if (!ModelState.IsValid)
            {
                model.Companies = await _context.Companies.ToListAsync();
                return View(model);
            }

            JobOffer jo = new JobOffer
            {
                CompanyId = model.CompanyId,
                Description = model.Description,
                JobTitle = model.JobTitle,
                Location = model.Location,
                SalaryFrom = model.SalaryFrom,
                SalaryTo = model.SalaryTo,
                ValidUntil = model.ValidUntil,
                Created = DateTime.Now
            };

            await _context.JobOffers.AddAsync(jo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var offer = await _context.JobOffers
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id);
            Role role = await AuthorizationTools.GetRoleAsync(User, _context);
            if (role == Role.HR)
            {
                JobOfferDetailsHRView jobOfferDetailsHRView = new JobOfferDetailsHRView();
                jobOfferDetailsHRView.Offer = offer;
                string email = AuthorizationTools.GetEmail(User);
                HR us = _context.HRs.Where(h => h.EmailAddress == email).First();
                jobOfferDetailsHRView.HR = us;
                jobOfferDetailsHRView.Applications = await _context.JobApplications.Where(ja => ja.JobOffer.HR == us).ToListAsync();
                return View("DetailsHR", jobOfferDetailsHRView);
            }
            if (role == Role.ADMIN)
                return View("DetailsAdmin", offer);
            return View("DetailsCandidate", offer);
        }
    }
}
