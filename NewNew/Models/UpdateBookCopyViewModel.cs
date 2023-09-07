using NewNew.Models.Domain;

namespace NewNew.Models
{
	public class UpdateBookCopyViewModel
	{
		public int bookCopyId { get; set; }
		public DateTime yearOfPublished { get; set; }
		public int PublisherId { get; set; }
		public Publisher Publisher { get; set; }
		public int bookId { get; set; }
		public Book Book { get; set; }
        public string bookCopyTitle { get; set; }

    }
}
