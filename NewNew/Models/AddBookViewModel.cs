using NewNew.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNew.Models
{
    public class AddBookViewModel
    {
        public int bookId { get; set; }
        public string bookTitle { get; set; }
        public string bookISBN { get; set; }
  
        public string bookDiscreption { get; set; }
        public int bookAmount { get; set; }
		public int bookCategroryId { get; set; }
            [NotMapped]
       public string categoryName { get; set; }
        public BookCategory BookCategory { get; set; }
        //public IFormFile ProfileImage { get; set; }
        public int authorId { get; set; }
        public string authorFullName { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; }
        public virtual ICollection<BookBorrow> Borrow { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Authors { get; set; }


    }
}
