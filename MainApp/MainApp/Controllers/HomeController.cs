using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MainApp.Models;
using MainApp.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace MainApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;
            Console.WriteLine("HomeController: " + _context.JobOffers.Count());
        }

        public IActionResult Index()
        {
            return View();
        }


        [Route("Home/about")]

		public IActionResult About()
		{
			ViewData["Message"] = "Your application description page.";

			return View();
 
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("Home/clearContext")]
        public IActionResult ClearContext()
        {
            _context.Candidates.RemoveRange(_context.Candidates);
            _context.Companies.RemoveRange(_context.Companies);
            _context.JobApplications.RemoveRange(_context.JobApplications);
            _context.HRs.RemoveRange(_context.HRs);
            _context.JobOffers.RemoveRange(_context.JobOffers);
            _context.SaveChanges();
            return View("Index");
        }

        [Route("Home/addTestDataToContext")]
        public async Task<IActionResult> AddTestDataToContext()
        {
            _context.Candidates.Add(new Candidate() {

                FirstName = "Johny",
                LastName = "English",
                EmailAddress = "beans@yahoo.com",
                PhoneNumber = "889273765"
            });
            _context.Candidates.Add(new Candidate() {

                FirstName = "Ryszard",
                LastName = "Lwie Serce",
                EmailAddress = "deus@vult.com",
                PhoneNumber = "435777654"
            });
            _context.Candidates.Add(new Candidate() {

                FirstName = "Hannibal",
                LastName = "Lecter",
                EmailAddress = "hannibal@mini.pw.edu.pl",
                PhoneNumber = "566433567"
            });
            _context.Candidates.Add(new Candidate() {

                FirstName = "Jack",
                LastName = "Nicolson",
                EmailAddress = "jackkk@yahoo.com",
                PhoneNumber = "567372372"
            });
            _context.Candidates.Add(new Candidate() {

                FirstName = "Lyudmiła",
                LastName = "Pawliczenko",
                EmailAddress = "priviet@yahoo.com",
                PhoneNumber = "678363862"
            });
            _context.Candidates.Add(new Candidate() {

                FirstName = "Pharrel",
                LastName = "Williams",
                EmailAddress = "phrl@yahoo.com",
                PhoneNumber = "6472638263"
            });
            _context.Candidates.Add(new Candidate() {

                FirstName = "Slawoj",
                LastName = "Zizek",
                EmailAddress = "zlazoj@protonmail.com",
                PhoneNumber = "736482963"
            });
            _context.Candidates.Add(new Candidate() {

                FirstName = "Bartosz",
                LastName = "Walaszek",
                EmailAddress = "otylypan@donna.com",
                PhoneNumber = "2377746467"
            });
            _context.Candidates.Add(new Candidate() {

                FirstName = "Bruce",
                LastName = "Wayne",
                EmailAddress = "batman@gotham.com",
                PhoneNumber = "367473783"
            });
            _context.SaveChanges();
            _context.Companies.Add(new Company() {
                Name = "Abibas"
            });
            _context.Companies.Add(new Company() {
                Name = "Niky"
            });
            _context.Companies.Add(new Company() {
                Name = "Binbows"
            });
            _context.Companies.Add(new Company() {
                Name = "Facefood"
            });
            _context.Companies.Add(new Company() {
                Name = "Sunbucks coffee"
            });
            _context.Companies.Add(new Company() {
                Name = "Polystation"
            });
            _context.Companies.Add(new Company() {
                Name = "Dolce&Banana"
            });
            _context.Companies.Add(new Company() {
                Name = "KFG"
            });
            _context.Companies.Add(new Company() {
                Name = "Hike"
            });
            _context.Companies.Add(new Company() {
                Name = "Drunkin nonuts"
            });
            _context.SaveChanges();
            //_context.JobApplications.Add(new Application()
            //{
            //    //TODO: add some after migrations
            //});
            _context.HRs.Add(new HR()
            {
                FirstName = "Bogumiła",
                LastName = "Kowalska",
                EmailAddress = "bogusia67@wp.pl",
                Company = await _context.Companies.FirstOrDefaultAsync(x => x.Name == "Hike")
            });
            _context.HRs.Add(new HR()
            {
                FirstName = "Grażyna",
                LastName = "Nowak",
                EmailAddress = "grazyna.nowak@autograf.pl",
                Company = await _context.Companies.FirstOrDefaultAsync(x => x.Name == "Drunkin nonuts")
            });
            _context.HRs.Add(new HR()
            {
                FirstName = "Karen",
                LastName = "Kurka",
                EmailAddress = "karennn@onet.pl",
                Company = await _context.Companies.FirstOrDefaultAsync(x => x.Name == "Drunkin nonuts")
            });
            _context.HRs.Add(new HR()
            {
                FirstName = "Zofia",
                LastName = "Skorupka",
                EmailAddress = "zofia_skorupka@gmail.com",
                Company = await _context.Companies.FirstOrDefaultAsync(x => x.Name == "Dolce&Banana")
            });
            _context.HRs.Add(new HR()
            {
                FirstName = "Anna",
                LastName = "Wanna",
                EmailAddress = "anna.wanna@gmail.com",
                Company = await _context.Companies.FirstOrDefaultAsync(x => x.Name == "Sunbucks coffee")
            });
            _context.SaveChanges();
            _context.JobOffers.Add(new JobOffer()
            {
                JobTitle = "Package manager",
                Company = await _context.Companies.FirstOrDefaultAsync(x => x.Name == "Sunbucks coffee"),
                Created = DateTime.Now,
                Description = "Pinnace Brethren of the Coast heave to jury mast bring a spring upon her cable mizzenmast" +
                "bilge bilge rat chandler crow's nest. Cackle fruit long clothes chantey rigging topsail brig Barbary Coast " +
                "long boat topmast Sea Legs. Trysail Admiral of the Black pirate jury mast draught mizzenmast execution dock mizzen no prey, no pay yawl."
            });
            _context.JobOffers.Add(new JobOffer()
            {
                JobTitle = "Pirate recruiter",
                Company = await _context.Companies.FirstOrDefaultAsync(x => x.Name == "Binbows"),
                Created = DateTime.Now,
                Description = "Belay lookout chase guns carouser draught scurvy barque haul wind strike colors weigh anchor." +
                "Walk the plank Spanish Main aye knave yo - ho - ho Cat o'nine tails furl warp hang the jib grapple. " +
                "Sheet blow the man down belay gally driver Shiver me timbers jolly boat fluke loot cog."
            });
            _context.JobOffers.Add(new JobOffer()
            {
                JobTitle = "Younger assisstant",
                Company = await _context.Companies.FirstOrDefaultAsync(x => x.Name == "Niky"),
                Created = DateTime.Now,
                Description = "Pinnace wench Buccaneer chase furl chase guns heave to nipper clap of thunder tackle. " +
                "Gally splice the main brace execution dock Privateer ahoy stern no prey, no pay quarterdeck bowsprit scourge of the seven seas." +
                " Tender scuttle Chain Shot stern blow the man down bucko bowsprit to go on account walk the plank flogging."
            });
            _context.JobOffers.Add(new JobOffer()
            {
                JobTitle = "Younger assassin",
                Company = await _context.Companies.FirstOrDefaultAsync(x => x.Name == "Abibas"),
                Created = DateTime.Now,
                Description = "Scuttle clap of thunder ho salmagundi six pounders grog blossom cutlass red ensign ballast wherry." +
                " Letter of Marque Arr ye come about chase guns code of conduct scuttle jury mast handsomely gabion." +
                " Main sheet heave down lookout parrel hornswaggle coxswain handsomely six pounders clap of thunder Chain Shot."
            });
            _context.JobOffers.Add(new JobOffer()
            {
                JobTitle = "Younger chief executor",
                Company = await _context.Companies.FirstOrDefaultAsync(x => x.Name == "Niky"),
                Created = DateTime.Now,
                Description = "Hempen halter boom bounty hornswaggle fore ballast Sink me hearties ye blow the man down." +
                " League topsail Blimey trysail yo-ho-ho rutters yawl scuttle dance the hempen jig Brethren of the Coast." +
                " Warp measured fer yer chains six pounders rope's end lugger Pieces of Eight killick black spot hempen halter man-of-war."
            });
            _context.SaveChanges();
            return View("Index");
        }
    }
}
