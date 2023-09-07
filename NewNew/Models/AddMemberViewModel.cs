using NewNew.Models.Domain;

namespace NewNew.Models
{
	public class AddMemberViewModel
	{
        public int  memberId { get; set; }
        public string memberFullName { get; set; }
		public string gender { get; set; }
		public string email { get; set; }
		public string phoneNumber { get; set; }
		public string memberPassword { get; set; }
        public MemberStatus Status { get; set; }
        public virtual ICollection<BookBorrow> Borrows { get; set; }
	}
}
