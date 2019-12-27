using System;
using System.Collections.Generic;
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
    public class CandidateController : Controller
    {
        private readonly DataContext _context;

        public CandidateController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index([FromQuery(Name = "search")] string searchString)
        {
            if (await AuthorizationTools.IsAdmin(User, _context) == false)
                return new UnauthorizedResult();
            if (string.IsNullOrEmpty(searchString))
                return View(await _context.Candidates.ToListAsync());

            List<Candidate> searchResult = await _context
                .Candidates
                .Where(o => o.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            return View(searchResult);
        }
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            string email = AuthorizationTools.GetEmail(User);
            Candidate us = _context.Candidates.Where(c => c.EmailAddress == email).First();
            if (await AuthorizationTools.IsAdmin(User, _context) == false || us == null || us.Id != id.Value)
                return new UnauthorizedResult();

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
        [Authorize]
        public async Task<ActionResult> Edit(Candidate model)
        {
            string email = AuthorizationTools.GetEmail(User);
            Candidate us = _context.Candidates.Where(c => c.EmailAddress == email).First();
            if (await AuthorizationTools.IsAdmin(User, _context) == false || us == null || us.Id != model.Id)
                return new UnauthorizedResult();

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
        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            if (await AuthorizationTools.IsAdmin(User, _context) == false)
                return new UnauthorizedResult();
            if (id == null)
            {
                return BadRequest($"id should not be null");
            }

            _context.Candidates.Remove(new Candidate() { Id = id.Value });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            string email = AuthorizationTools.GetEmail(User);
            Candidate us = _context.Candidates.Where(c => c.EmailAddress == email).First();
            if (await AuthorizationTools.IsAdmin(User, _context) == false || us == null || us.Id != id)
                return new UnauthorizedResult();

            Role role = await AuthorizationTools.GetRoleAsync(User, _context);
            var candidate = await _context.Candidates
                .FirstOrDefaultAsync(x => x.Id == id);
            if (role == Role.ADMIN)
                return View("DetailsAdmin", candidate);
            return View("DetailsCandidate", candidate);
        }
    }
}