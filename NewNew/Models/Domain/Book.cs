


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewNew.Models.Domain

{
    public class Book
  {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int bookId { get; set; } 
        public string bookTitle { get; set; } 
        public string bookISBN { get; set; }
        public string bookDiscreption { get; set; } 
        public int bookAmount { get; set; } 
        public int bookCategroryId { get; set; } 
        public BookCategory BookCategory { get; set; }
        [NotMapped]
        public string categoryName { get; set; }
        //   public string ProfileImage { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; } 
        public ICollection<Reservation> Reservations { get; set; } 
        public ICollection<BookCopy> bookCopies { get; set; }
       
    }
}
