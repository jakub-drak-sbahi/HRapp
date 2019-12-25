using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MainApp.EntityFramework;
using MainApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainApp.Controllers
{
    public class CompanyController : Controller
    {

        private readonly DataContext _context;
        public CompanyController(DataContext context)
        {
            _context = context;
            Console.WriteLine("CompanyController: " + _context.JobOffers.Count());
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery(Name = "search")] string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
                return View(await _context.Companies.ToListAsync());

            List<Company> searchResult = await _context
                .Companies
                .Where(o => o.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            return View(searchResult);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            Company company = await _context.Companies.FindAsync(id);
            if(company==null)
            {
                return NotFound();
            }
            return View(company);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Company company = _context.Companies.Find(id);
            if(company==null)
            {
                return NotFound();
            }
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateConfirmed(Company company)
        {
            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            Company company = await _context.Companies.FindAsync(id);
            if(company==null)
            {
                return NotFound();
            }
            return View(company);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditConfirmed(Company company)
        {
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index"); 
        }

        public IActionResult Details(int id)
        {
            var model = _context.Companies.Find(id);
            return View("Details", model);
        }
    }
}