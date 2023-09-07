using NewNew.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewNew.Models
{
    public class AddFineViewModel
    {
        public int FineId { get; set; }
        public int FineAmount { get; set; }
        public DateTime FineDate { get; set; }
        public int? borrowBookId { get; set; }
        public string bookTitle { get; set; }
        public BookBorrow BookBorrow { get; set; }
        public FinePayment FinePayment { get; set; }
		public IEnumerable<SelectListItem> bBorrow { get; set; }
	}
}
