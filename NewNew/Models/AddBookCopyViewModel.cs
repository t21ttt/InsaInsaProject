using NewNew.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNew.Models
{
	public class AddBookCopyViewModel
	{
		public int bookCopyId { get; set; }
		public DateTime yearOfPublished { get; set; }
		public int PublisherId { get; set; }
        [NotMapped]
        public string publisherName { get; set; }
        [NotMapped]
        public string bookTitle { get; set; }
        public Publisher Publisher { get; set; }
		public int bookId { get; set; }
		public Book Book { get; set; }

		public List<SelectListItem> Books { get; set; }
		public List<SelectListItem> Publishers { get; set; }
	}
}
