using NewNew.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewNew.Models
{
    public class AddReservationViewModel
    {
        public int ReservationId { get; set; }
        public int? memberId { get; set; }
        public Member Member { get; set; }
        public int? bookId { get; set; }
        public Book Book { get; set; }
        public DateTime reservatonDate { get; set; }
		public List<SelectListItem> Members { get; set; }
		public List<SelectListItem> Books{ get; set; }
	}
}
