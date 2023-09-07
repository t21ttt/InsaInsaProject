using NewNew.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewNew.Models
{
    public class AddBorrowBookViewModel
    {
        public int borrowBookId { get; set; }
        public DateTime borrowBookDate { get; set; }
        public DateTime borrowDueDate { get; set; }
        public int? bookCopyId { get; set; }
        public BookCopy BookCopy { get; set; }
        public int? memberId { get; set; }
        public Member Member { get; set; }
        public bool isReturned { get; set; }
		public bool isNotified { get; set; }
		public ICollection<Fine> fines { get; set; }
        public IEnumerable<SelectListItem> Members { get; set; }
        public IEnumerable<SelectListItem> Bcopy { get; set; }

    }
}
