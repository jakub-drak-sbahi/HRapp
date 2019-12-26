using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MainApp.Models;
using MainApp.EntityFramework;
using Microsoft.EntityFrameworkCore;
using MainApp.Authorization;

namespace MainApp.Controllers
{
    public class CandidateController : Controller
    {
        private readonly DataContext _context;

        public CandidateController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery(Name = "search")] string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
                return View(await _context.Candidates.ToListAsync());

            List<Candidate> searchResult = await _context
                .Candidates
                .Where(o => o.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            return View(searchResult);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest($"id shouldn't not be null");
            }
            var offer = await _context.Candidates.FirstOrDefaultAsync(x => x.Id == id.Value);
            if (offer == null)
            {
                return NotFound($"offer not found in DB");
            }

            return View(offer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Candidate model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var candidate = await _context.Candidates.FirstOrDefaultAsync(x => x.Id == model.Id);
            candidate.FirstName = model.FirstName;
            candidate.LastName = model.LastName;
            candidate.EmailAddress = model.EmailAddress;
            candidate.PhoneNumber = model.PhoneNumber;
            _context.Update(candidate);
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

            _context.Candidates.Remove(new Candidate() { Id = id.Value });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Create()
        {
            //don't know yet, should it be connected to Authorization profile?
            throw new System.NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(JobOfferCreateView model)
        {
            //don't know yet, should it be connected to Authorization profile?
            throw new System.NotImplementedException();
        }

        public async Task<IActionResult> Details(int id)
        {
            Role role = Role.CANDIDATE;
            var candidate = await _context.Candidates
                .FirstOrDefaultAsync(x => x.Id == id);
            if(role == Role.ADMIN)
                return View("DetailsAdmin", candidate);
            return View("DetailsCandidate", candidate);
        }
    }
}