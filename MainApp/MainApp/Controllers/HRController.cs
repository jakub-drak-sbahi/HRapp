using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainApp.Authorization;
using MainApp.EntityFramework;
using MainApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainApp.Controllers
{
    public class HRController : Controller
    {
        private readonly DataContext _context;

        public HRController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery(Name = "search")] string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
                return View(await _context.HRs.Include(x => x.Company).ToListAsync());

            List<HR> searchResult = await _context
                .HRs.Include(x => x.Company)
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
            var hr = await _context.HRs.FirstOrDefaultAsync(x => x.Id == id.Value);
            if (hr == null)
            {
                return NotFound($"HR not found in DB");
            }

            return View(hr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(HR model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var hr = await _context.HRs.FirstOrDefaultAsync(x => x.Id == model.Id);
            hr.FirstName = model.FirstName;
            hr.LastName = model.LastName;
            hr.Company = model.Company;
            _context.Update(hr);
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

            _context.HRs.Remove(new HR() { Id = id.Value });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Create()
        {
            var model = new HRCreateView
            {
                Companies = await _context.Companies.ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(HRCreateView model)
        {
            Console.WriteLine("dupa hehe");
            if (!ModelState.IsValid)
            {
                model.Companies = await _context.Companies.ToListAsync();
                return View(model);
            }

            HR hr = new HR
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                CompanyId = model.CompanyId
            };

            await _context.HRs.AddAsync(hr);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            Role role = Role.HR;
            var hr = await _context.HRs
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id);
            if(role == Role.ADMIN)
                return View("DetailsAdmin", hr);
            return View("DetailsHR", hr);
        }
    }
}