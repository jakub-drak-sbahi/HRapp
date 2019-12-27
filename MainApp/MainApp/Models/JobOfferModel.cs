using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MainApp.Models
{
    public class JobOffer
    {
        public int Id { get; set; }
        [Display(Name = "Job title")]
        [Required]
        public string JobTitle { get; set; }
        public virtual Company Company { get; set; }    // to jest niepotrzebne, nie usuwalem na wszelki wypadek
        public virtual int CompanyId { get; set; }     // to jest jeszcze bardziej niepotrzebne, bo powyzsze samo dodaje w bazie CompanyId, analogicznie jest w pozostalych miejscach jednak na wszelki wypadek jeszcze tego nie usunalem zeby uniknac nieprzeiwdzianych bledow
        [Display(Name = "Salary from")]
        public decimal? SalaryFrom { get; set; }
        [Display(Name = "Salary to")]
        public decimal? SalaryTo { get; set; }
        public DateTime Created { get; set; }
        public string Location { get; set; }
        [Required]
        [MinLength(5)]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        [Display(Name = "Valid until")]
        public DateTime? ValidUntil { get; set; }
        public List<Application> JobApplications { get; set; } = new List<Application>();
        public HR HR { get; set; }
    }
}
