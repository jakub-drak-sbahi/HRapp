using MainApp.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MainApp.Authorization
{
    public enum Role { CANDIDATE, HR, ADMIN }
    public static class AuthorizationTools
    {
        public static string GetEmail(ClaimsPrincipal User)
        {
            return User.Claims.ToList()[1].Value;
        }

        public static async Task<Role> GetRoleAsync(ClaimsPrincipal User, DataContext context)
        {
            string email = GetEmail(User);
            var admin = await context.Admins.Where(a => a.EmailAddress == email).ToListAsync();
            if (admin.Count() > 0)
                return Role.ADMIN;
            var hr = await context.HRs.Where(h => h.EmailAddress == email).ToListAsync();
            if (hr.Count() > 0)
                return Role.HR;
            var candidate = await context.Candidates.Where(c => c.EmailAddress == email).ToListAsync();
            if (candidate.Count() == 0)
            {
                await context.Candidates.AddAsync(new Models.Candidate() { EmailAddress = email });
                await context.SaveChangesAsync();
            }
            return Role.CANDIDATE;
        }

        public static async Task<bool> IsCandidate(ClaimsPrincipal User, DataContext context)
        {
            Role role = await GetRoleAsync(User, context);
            return role == Role.CANDIDATE;
        }

        public static async Task<bool> IsHR(ClaimsPrincipal User, DataContext context)
        {
            Role role = await GetRoleAsync(User, context);
            return role == Role.HR;
        }

        public static async Task<bool> IsAdmin(ClaimsPrincipal User, DataContext context)
        {
            Role role = await GetRoleAsync(User, context);
            return role == Role.ADMIN;
        }

        public static string AddViewSuffix(string viewName, Role role)
        {
            //maybe this way?
            if (role == Role.ADMIN)
                return viewName + "Admin";
            if (role == Role.HR)
                return viewName + "HR";
            return viewName + "Candidate";
        }
    }
}


//********PRZYKLAD UZYCIA*************

//public class JobOfferController : Controller
//{
//    private readonly DataContext _context;

//    public JobOfferController(DataContext context)
//    {
//        _context = context;
//    }

//    [HttpGet]
//    [Authorize]
//    public async Task<IActionResult> Index([FromQuery(Name = "search")] string searchString)
//    {
//        if (await MainApp.Authorization.AuthorizationTools.IsAdmin(User, _context) == false)
//            return new UnauthorizedResult();

//        if (string.IsNullOrEmpty(searchString))
//            return View(await _context.JobOffers.Include(x => x.Company).ToListAsync());

//        List<JobOffer> searchResult = await _context
//            .JobOffers.Include(x => x.Company)
//            .Where(o => o.JobTitle.Contains(searchString, StringComparison.OrdinalIgnoreCase))
//            .ToListAsync();

//        return View(searchResult);
//    }
//}
