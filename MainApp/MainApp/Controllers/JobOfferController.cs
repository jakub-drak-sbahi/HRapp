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
        public async Task<IActionResult> Index([FromQuery(Name = "search")] string searchString)
        {
            Role role = Role.HR;
            string viewName = AuthorizationTools.AddViewSuffix("Index", role);
            List<JobOffer> searchResult;
            if (string.IsNullOrEmpty(searchString))
            {
                searchResult = await _context.JobOffers.Include(x => x.Company).ToListAsync();
            }
            else
            {
                searchResult = await _context
                    .JobOffers.Include(x => x.Company)
                    .Where(o => o.JobTitle.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToListAsync();
            }
            if (role == Role.HR)
            {
                JobOfferIndexHRView jobOfferIndexHRView = new JobOfferIndexHRView();
                jobOfferIndexHRView.Offers = searchResult;
                //jobOfferIndexHRView.HR = thishr;
                jobOfferIndexHRView.HR = new HR();
                return View(viewName, jobOfferIndexHRView);
            }
            else if(role == Role.CANDIDATE)
            {
                JobOfferIndexCandidateView jobOfferIndexCandidateView = new JobOfferIndexCandidateView();
                jobOfferIndexCandidateView.Offers = searchResult;
                //jobOfferIndexCandidateView.Candidate = thiscandidate;
                jobOfferIndexCandidateView.Candidate = new Candidate();
                return View(viewName, jobOfferIndexCandidateView);
            }
            //role == Role.ADMIN
            return View(viewName, searchResult);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest($"id shouldn't be null");
            }
            var offer = await _context.JobOffers.FirstOrDefaultAsync(x => x.Id == id.Value);
            if (offer == null)
            {
                return NotFound($"offer not found in DB");
            }

            return View(offer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(JobOffer model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var offer = await _context.JobOffers.FirstOrDefaultAsync(x => x.Id == model.Id);
            offer.JobTitle = model.JobTitle;
            offer.Description = model.Description;
            _context.Update(offer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = model.Id });
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest($"id should not be null");
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
            Role role = Role.HR;
            string viewName = AuthorizationTools.AddViewSuffix("Details", role);
            if (role == Role.HR)
            {
                JobOfferDetailsHRView jobOfferDetailsHRView = new JobOfferDetailsHRView();
                jobOfferDetailsHRView.Offer = offer;
                //jobOfferIndexHRView.HR = thishr;
                jobOfferDetailsHRView.HR = new HR()
                {
                    Company = offer.Company
                };
                //jobOfferDetailsHRView.Applications = await _context.JobApplications.Where(x => x.HR == thishr).ToListAsync();
                jobOfferDetailsHRView.Applications = await _context.JobApplications.ToListAsync();
                return View(viewName, jobOfferDetailsHRView);
            }
            return View(viewName, offer);
        }
    }
}
