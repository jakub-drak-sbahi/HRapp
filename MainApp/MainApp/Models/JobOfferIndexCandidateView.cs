using System.Collections.Generic;

namespace MainApp.Models
{
	public class JobOfferIndexCandidateView : JobOffer 
	{
		public IEnumerable<JobOffer> Offers { get; set; }
		public Candidate Candidate { get; set; }
	}
}
