using System.Collections.Generic;

namespace MainApp.Models
{
	public class JobOfferCreateView : JobOffer 
	{
		public IEnumerable<Company> Companies { get; set; }
	}
}
