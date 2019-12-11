﻿using System;
using System.Collections.Generic;
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

        public IActionResult Index()
        {
            return View("Index", _context.JobApplications);
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
    }
}
