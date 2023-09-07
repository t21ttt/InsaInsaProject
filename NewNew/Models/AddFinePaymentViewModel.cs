using NewNew.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewNew.Models
{
    public class AddFinePaymentViewModel
    {
        public int finePaymentId { get; set; }
        public int finePaymentAmount { get; set; }
        public DateTime finePaymentDate { get; set; }
        public int? memberId { get; set; }
        public Member Member { get; set; }
        public int FineId { get; set; }
        public Fine Fine { get; set; }
        public bool isPaid { get; set; }
        public List<SelectListItem> Members { get; set; }
		public List<SelectListItem> fines { get; set; }

	}
}
