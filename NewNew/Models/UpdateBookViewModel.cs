using NewNew.Models.Domain;

namespace NewNew.Models
{
	public class UpdateBookViewModel
	{
  
		public int BookId { get; set; }
        public string bookTitle { get; set; }
		public string bookISBN { get; set; }
		public byte[] bookImage { get; set; }
		public string bookDiscreption { get; set; }
		public int bookAmount { get; set; }
		public int bookCategoryId { get; set; }
       // public IFormFile ProfileImage { get; set; }

        public virtual ICollection<BookBorrow> Borrows { get; set; }

}

}
