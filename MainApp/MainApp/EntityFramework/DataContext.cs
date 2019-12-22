using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainApp.Models;

namespace MainApp.EntityFramework
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Application> JobApplications { get; set; }
        public DbSet<JobOffer> JobOffers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<HR> HRs { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}
